using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.Material;

/// <summary>
/// A standard material with default values that models the Cook-Torrance Shading Model.
/// </summary>
public class Shader(Scene scene)
{
    private const ushort MaxDepth = 6;
    private const float Epsilon = 0.001f;
    
    private const float ReflectionContribution = 0.1f;
    private const float RefractionContribution = 0.1f;
    private const float ShadingContribution = 1.0f - ReflectionContribution - RefractionContribution;
    private float[] AmbientColor { get; } = [0.1f, 0.1f, 0.1f, 1.0f];
    private Scene Scene { get; set; } = scene;
    
    // Vector s -> From the light source to the hit point => s = lightDirection - hitPoint.Position
    // Vector v -> Points to the camera => v = -rayDirection
    // Vector m -> normal to the surface at the hit point
    // Vector h -> Halfway vector between s and v => h = (s + v)
    
    public float[] Shade(Ray ray, ushort depth = 0)
    {
        if (depth > MaxDepth) return [0, 0, 0, 0]; // Stop at max depth
        
        // Get the hit point
        var hitPoint = Scene.GetBestHit(ray);
        if (hitPoint == null) return AmbientColor; // Return the ambient color if no hit
        
        // Create a feeler ray to check for shadows with a small offset
        var feelerRay = new Ray(hitPoint.Point - Epsilon * ray.Direction, Vector3.Zero);
        
        // Calculate some vectors which are not dependent on the light
        var v = Vector3.Normalize(-ray.Direction); // Camera direction
        var NdotV = Vector3.Dot(hitPoint.Normal, v);
        
        // Specify the factors of contribution for the different light components
        var kD = hitPoint.Object.Material.SurfaceRoughness; // The diffuse contribution
        var kS = 1 - kD; // The specular contribution
        
        // Get the color values associated with the material at the hitpoint
        var materialColor = hitPoint.Object.Material.GetDiffuseColor(hitPoint);
        var colorValues = hitPoint.Object.Material.GetAmbientColor(hitPoint);
        
        // Next we need to compute the shading for each light source
        foreach (var light in Scene.Lights)
        {
            // Compute a direction for the "feeler" ray to check for shadows
            feelerRay.Direction = light.GlobalPosition - hitPoint.Point;
            if (Scene.IsInShadow(feelerRay))
            {
                // When in shadow, skip the light source calculation
                continue;
            }
        
            var s = Vector3.Normalize(feelerRay.Direction); // Light direction
            var h = Vector3.Normalize(v + s); // Halfway vector
        
            var NdotS = Vector3.Dot(hitPoint.Normal, s);
            var NdotH = Vector3.Dot(hitPoint.Normal, h);
            //var VdotH = Vector3.Dot(v, h);
            var HdotS = Vector3.Dot(h, s);

            // Combine some terms to speed up calculations
            var DtimesG = CookTorrance.NormalDistributionFunction(NdotH, hitPoint.Object.Material.SurfaceRoughness)
                          * CookTorrance.Geometry(NdotH, NdotS, NdotV, HdotS);

            // For each RGBA-channel, calculate the color value
            for (var i = 0; i < 3; i++)
            {
                var specular = light.Color[i] * kS * CookTorrance.FresnelFunction(NdotS, hitPoint.Object.Material.EtaFresnel[i]) * DtimesG / NdotV;
                var diffuse = light.Color[i] * kD * materialColor[i];

                colorValues[i] += specular + diffuse;
            }
        }
        
        // Check if the object is reflective enough to warrant a reflection
        float[] reflectionColors = [0.0f, 0.0f, 0.0f, 1.0f];
        /*if (hitPoint.Object.Material.ReflectionCoefficient > 0.4f)
        {
            reflectionColors = CalculateReflections(hitPoint, ray, depth);
        }*/
        
        // Refraction
        float[] refractedColors = [0.0f, 0.0f, 0.0f, 1.0f];
        /*if (hitPoint.Object.Material.TransparencyCoefficient > 0.5f)
        {
            refractedColors = CalculateRefraction(hitPoint, ray, depth);
        }*/
        
        // Sum all together with their respective coefficients
        colorValues[0] = ShadingContribution * colorValues[0]
                          + hitPoint.Object.Material.ReflectionCoefficient * reflectionColors[0] * ReflectionContribution
                          + hitPoint.Object.Material.RefractionIndex * refractedColors[0] * RefractionContribution;
        colorValues[1] = ShadingContribution * colorValues[1]
                          + hitPoint.Object.Material.ReflectionCoefficient * reflectionColors[1] * ReflectionContribution
                          + hitPoint.Object.Material.RefractionIndex * refractedColors[1] * RefractionContribution;
        colorValues[2] = ShadingContribution * colorValues[2]
                          + hitPoint.Object.Material.ReflectionCoefficient * reflectionColors[2] * ReflectionContribution
                          + hitPoint.Object.Material.RefractionIndex * refractedColors[2] * RefractionContribution;
        
        
        // Clamp RGB values between 0-1 and return
        return [
            System.Math.Clamp(colorValues[0], 0.0f, 1.0f),
            System.Math.Clamp(colorValues[1], 0.0f, 1.0f),
            System.Math.Clamp(colorValues[2], 0.0f, 1.0f),
            1.0f
        ];
    }

    private float[] CalculateReflections(HitPoint hitPoint, Ray ray, ushort depth)
    {
        var reflectionRay = new Ray(
            hitPoint.Point - Epsilon * ray.Direction, // Offset the ray slightly to avoid self-intersection
            Vector3.Normalize(
                ray.Direction - 2 * Vector3.Dot(ray.Direction, hitPoint.Normal) * hitPoint.Normal
            ));
        
        return Shade(reflectionRay, (ushort) (depth + 1));
    }
    
    private float[] CalculateRefraction(HitPoint hitPoint, Ray ray, ushort depth)
    {
        var refractedRay = new Ray(Vector3.Zero, Vector3.Zero, ray.InsideObjects ?? []);
        var cValues = GetRefractiveIndexesFromRay(hitPoint, ray, refractedRay);
        
        var cFraction = cValues[1] / cValues[0]; // c2 / c1
        var NdotI = Vector3.Dot(hitPoint.Normal, ray.Direction);
        var cosTheta2 = System.MathF.Sqrt(1 - MathF.Pow(cFraction, 2) * (1 - MathF.Pow(NdotI, 2))); // 12.46 in the book
        
        // Check if the ray is totally internally reflected
        if (cosTheta2 < 0) return [0.0f, 0.0f, 0.0f, 1.0f];
        
        // Calculate the refracted ray direction
        refractedRay.Direction = cFraction * ray.Direction + (cFraction * NdotI - cosTheta2) * hitPoint.Normal;
        return Shade(refractedRay, (ushort) (depth + 1));
    }

    /// <summary>
    /// Returns the correct c1 and c2 for the refraction calculation.
    /// Also updates the refractive indexes list. 
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="ray"></param>
    /// <param name="refractedRay"></param>
    /// <returns></returns>
    private float[] GetRefractiveIndexesFromRay(HitPoint hitPoint, Ray ray, Ray refractedRay)
    {
        // Check if we are entering or exiting the object and add or remove the object from the list respectively
        if (hitPoint.IsEntering)
        {
            refractedRay.InsideObjects.Add(hitPoint.Object);
        }
        else
        {
            refractedRay.InsideObjects.Remove(hitPoint.Object);
        }
        
        // From the original ray find c1, if no objects use air. Otherwise, use the latest object.
        var c1 = ray.InsideObjects == null || ray.InsideObjects.Count == 0 ? 1.0f 
            : ray.InsideObjects[^1].Material.RefractionIndex;
        
        // Same for c2 but then for the refracted ray
        var c2 = refractedRay.InsideObjects.Count == 0 ? 1.0f
            : refractedRay.InsideObjects[^1].Material.RefractionIndex;
        
        return [c1, c2];
    }
}