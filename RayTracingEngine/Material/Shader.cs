using System.Numerics;
using RayTracingEngine.Rendering;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.Material;

/// <summary>
/// A standard material with default values that models the Cook-Torrance Shading Model.
/// </summary>
public class Shader(Scene scene)
{
    private const UInt16 MaxDepth = 0;
    private readonly float[] _colorValues = new float[3];
    public Vector3 AmbientColor { get; set; } = new(0.1f, 0.1f, 0.1f);
    public Scene Scene { get; set; } = scene;
    
    // Vector s -> From the light source to the hit point => s = lightDirection - hitPoint.Position
    // Vector v -> Points to the camera => v = -rayDirection
    // Vector m -> normal to the surface at the hit point
    // Vector h -> Halfway vector between s and v => h = (s + v)
    
    public Rgba32 Shade(Ray ray, ushort depth = 0)
    {
        if (depth > MaxDepth) return new Rgba32(0, 0, 0); // Stop at max depth
        
        // Get the hit point
        var hitPoint = Scene.GetBestHit(ray);
        if (hitPoint == null) return new Rgba32(0, 0, 0);
        
        // Create a feeler ray to check for shadows with a small offset
        const float epsilon = 0.001f;
        var feelerRay = new Ray(hitPoint.Point - epsilon * ray.Direction, Vector3.Zero);
        
        // Calculate some vectors which are not dependent on the light
        var v = Vector3.Normalize(-ray.Direction); // Camera direction
        var NdotV = Vector3.Dot(hitPoint.Normal, v);
        
        // Reset color values to ambient color
        _colorValues[0] = AmbientColor.X;
        _colorValues[1] = AmbientColor.Y;
        _colorValues[2] = AmbientColor.Z;

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

            // For each RGB-channel, calculate the color value
            for (var i = 0; i < 3; i++)
            {
                // kD is dependant on our surface roughness
                var kD = hitPoint.Object.Material.SurfaceRoughness;
                var kS = 1 - kD;
            
                var specular = light.Color[i] * kS * CookTorrance.FresnelFunction(NdotS, hitPoint.Object.Material.EtaFresnel[i]) * DtimesG / NdotV;
                var diffuse = light.Color[i] * kD * hitPoint.Object.Material.DiffuseColor[i];

                _colorValues[i] += specular + diffuse;
            }
        }
        
        // Check if the object is reflective enough to warrant a reflection
        if (hitPoint.Object.Material.ReflectionCoefficient > 0.6f)
        {
            // Calculate the reflections
            var reflectionColors = CalculateReflections(hitPoint, ray, depth);
            
            // Add the color values from the reflections
            _colorValues[0] += hitPoint.Object.Material.ReflectionCoefficient * reflectionColors.R;
            _colorValues[1] += hitPoint.Object.Material.ReflectionCoefficient * reflectionColors.G;
            _colorValues[2] += hitPoint.Object.Material.ReflectionCoefficient * reflectionColors.B;
        }
        
        // Clamp RGB values between 0-1 and return
        return new Rgba32(
            System.Math.Clamp(_colorValues[0], 0.0f, 1.0f),
            System.Math.Clamp(_colorValues[1], 0.0f, 1.0f),
            System.Math.Clamp(_colorValues[2], 0.0f, 1.0f)
        );
    }

    private Rgba32 CalculateReflections(HitPoint hitPoint, Ray ray, ushort depth)
    {
        var reflectionRay = new Ray(hitPoint.Point, Vector3.Reflect(ray.Direction, hitPoint.Normal));
        
        return Shade(reflectionRay, (ushort) (depth + 1));
    }
}