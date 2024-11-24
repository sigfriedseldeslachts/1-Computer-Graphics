using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.Material;

/// <summary>
/// A standard material with default values that models the Cook-Torrance Shading Model.
/// </summary>
public class AMaterial
{
    public Vector3 FresnelColor { get; set; } = new(0.95f, 0.93f, 0.88f);
    
    // Vector s -> From the light source to the hit point => s = lightDirection - hitPoint.Position
    // Vector v -> Points to the camera => v = -rayDirection
    // Vector m -> normal to the surface at the hit point
    // Vector h -> Halfway vector between s and v => h = (s + v)
    
    public Rgba32 Shade(HitPoint hitPoint, List<Light> lights, Vector3 rayDirection)
    {
        var light = lights.First();
        
        var v = Vector3.Normalize(-rayDirection); // Camera direction
        var s = Vector3.Normalize(light.Direction - hitPoint.Point); // Light direction
        var h = Vector3.Normalize(v + s); // Halfway vector
        
        var NdotS = Vector3.Dot(hitPoint.Normal, s);
        var NdotV = Vector3.Dot(hitPoint.Normal, v);
        var NdotH = Vector3.Dot(hitPoint.Normal, h);
        var VdotH = Vector3.Dot(v, h);

        // Calculate the Fresnel term using Schlick's approximation
        var fresnel = FresnelFunction(VdotH);

        // Calculate the geometry term with Schlick's approximation
        var geometry = GeometrySmith(NdotV, NdotS, SurfaceRoughness());

        // Calculate the normal distribution function
        var distribution = NormalDistributionFunction(NdotH, SurfaceRoughness());

        // Combine terms to get the Cook-Torrance specular component
        var specular = (distribution * geometry * fresnel) / (4 * NdotV * NdotS + 1e-5f);

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

    private float FresnelFunction(float VdotH)
    {
        return FresnelColor.X + (1 - FresnelColor.X) * MathF.Pow(1 - VdotH, 5);
    }

    private float NormalDistributionFunction(float delta, float roughness)
    {
        // Using Beckmanm63 distribution
        var a = roughness * roughness;
        return 1 / (4 * MathF.Pow(roughness, 2) * MathF.Pow(MathF.Cos(delta), 4));
    }

    private float GeometrySmith(float NdotV, float NdotL, float roughness)
    {
        var ggxV = GeometrySchlickGGX(NdotV, roughness);
        var ggxL = GeometrySchlickGGX(NdotL, roughness);
        return ggxV * ggxL;
    }

    private float GeometrySchlickGGX(float NdotX, float roughness)
    {
        var k = (roughness + 1) * (roughness + 1) / 8;
        return NdotX / (NdotX * (1 - k) + k);
    }

    private float SurfaceRoughness()
    {
        return 0.5f;
    }
}