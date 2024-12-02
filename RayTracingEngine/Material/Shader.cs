using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.Material;

/// <summary>
/// A standard material with default values that models the Cook-Torrance Shading Model.
/// </summary>
public class Shader
{
    public Vector3 AmbientColor = new(0.1f, 0.1f, 0.1f);
    
    // Vector s -> From the light source to the hit point => s = lightDirection - hitPoint.Position
    // Vector v -> Points to the camera => v = -rayDirection
    // Vector m -> normal to the surface at the hit point
    // Vector h -> Halfway vector between s and v => h = (s + v)
    
    public Rgba32 Shade(HitPoint hitPoint, List<Light> lights, Vector3 rayDirection)
    {
        var light = lights.First();

        var material = new GoldMaterial();
        
        var v = Vector3.Normalize(-rayDirection); // Camera direction
        var s = Vector3.Normalize(light.Direction - hitPoint.Point); // Light direction
        var h = Vector3.Normalize(v + s); // Halfway vector
        
        var NdotS = Vector3.Dot(hitPoint.Normal, s);
        var NdotV = Vector3.Dot(hitPoint.Normal, v);
        var NdotH = Vector3.Dot(hitPoint.Normal, h);
        var VdotH = Vector3.Dot(v, h);
        var HdotS = Vector3.Dot(h, s);

        // Calculate the Fresnel term using Schlick's approximation
        var fresnel = FresnelFunction(NdotS);

        // Calculate the geometry term with Cook-Toorance model
        var geometry = Geometry(NdotH, NdotS, NdotV, HdotS);

        // Calculate the normal distribution function
        var distribution = NormalDistributionFunction(NdotH, SurfaceRoughness());

        // Combine terms to get the Cook-Torrance specular component
        var specular = fresnel * distribution * geometry / NdotV;

        // Scale to RGB output (clamp to ensure values remain in range)
        var colorValue = 255 * specular * NdotS * light.Intensity;
        colorValue = colorValue switch
        {
            > 255 => 255,
            < 0 => 0,
            _ => colorValue
        };

        // Map the value from 0-255 to a 0-1 range
        colorValue /= 255;

        return new Rgba32(colorValue, colorValue, colorValue);
    }

    /// <summary>
    /// Fresnel function which models the reflectance of the surface.
    /// </summary>
    /// <param name="c">Dot product of surface normal and s</param>
    /// <returns></returns>
    private float FresnelFunction(float c)
    {
        var refractionIndex = 20.0f;
        var g = MathF.Sqrt(MathF.Pow(refractionIndex, 2) + MathF.Pow(c, 2) - 1);

        var gPc = g + c;
        var gMc = g - c;

        var secondFraction = (c * gPc - 1) / (c * gMc + 1);
        return 0.5f * MathF.Pow(gMc / gPc, 2) * (1 + MathF.Pow(secondFraction, 2));
    }

    /// <summary>
    /// Beckmann 63 distribution function which models the microfacets of the surface.
    /// </summary>
    /// <param name="cosDelta">cosin of the angle relative to surface normal (Dot product of N and H)</param>
    /// <param name="roughness">Measure of surface roughness</param>
    /// <returns></returns>
    private float NormalDistributionFunction(float cosDelta, float roughness)
    {
        var nom = MathF.Exp(-MathF.Pow(MathF.Tan(MathF.Acos(cosDelta)) / roughness, 2));
        return nom / (4 * MathF.Pow(roughness, 2) * MathF.Pow(cosDelta, 4));
    }

    private float Geometry(float NdotH, float NdotS, float NdotV, float HdotS)
    {
        // Using Cook-Torrance model
        var gm = 2 * NdotH * NdotS / HdotS;
        var gs = 2 * NdotH * NdotV / HdotS;
        return MathF.Min(1, MathF.Min(gm, gs));
    }
    
    private float SurfaceRoughness()
    {
        return 0.5f;
    }
}