using System.Numerics;
using RayTracingEngine.Rendering;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.Material;

/// <summary>
/// A standard material with default values that models the Cook-Torrance Shading Model.
/// </summary>
public class AMaterial
{
    public Rgba32 Shade(HitPoint hitPoint, Vector3 lightDirection, Vector3 viewDirection, Vector3 halfway)
    {
        var phi = Vector3.Dot(hitPoint.Normal, lightDirection); // Angle between normal and light direction
        var theta = Vector3.Dot(hitPoint.Normal, viewDirection); // Angle between normal and view direction
        var delta = (theta - phi) / 2; // Halfway vector

        float specularLevel = NormalDistributionFunction(delta)
            * GeometryFunction(phi, theta, delta)
            * FresnelFunction(phi) / phi;
        
        return new Rgba32(
            (byte) (255 * specularLevel),
            (byte) (255 * specularLevel),
            (byte) (255 * specularLevel)
        );
    }

    /// <summary>
    /// The concentration of microfacets that can reflict light. We use the Beckmann distribution function.
    /// </summary>
    /// <param name="delta">Angle of the light direction</param>
    /// <returns></returns>
    public float NormalDistributionFunction(float delta)
    {
        return 1 / (4 * MathF.Pow(SurfaceRoughness(), 2) * MathF.Pow(delta, 4))
               * MathF.Exp(
                   -MathF.Pow(
                       MathF.Tan(delta), 2) / MathF.Pow(SurfaceRoughness()
                       , 2)
                );
    }
    
    public float GeometryFunction(float phi, float theta, float delta)
    {
        return 0.5f;
        //var g_m = 2 * delta * phi / theta;
        //var g_s = 2 * delta * 
    }
    
    public float FresnelFunction(float angle)
    {
        return 0.5f;
    }
    
    public float SurfaceRoughness()
    {
        return 0.5f;
    }
    
    public float FresnelCoefficient(float angle, float n1, float n2)
    {
        return 0.0f;
    }
}