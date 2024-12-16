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
    private const UInt16 MaxDepth = 5;
    private readonly float[] _colorValues = new float[3];
    public Vector3 AmbientColor = new(0.1f, 0.1f, 0.1f);
    
    // Vector s -> From the light source to the hit point => s = lightDirection - hitPoint.Position
    // Vector v -> Points to the camera => v = -rayDirection
    // Vector m -> normal to the surface at the hit point
    // Vector h -> Halfway vector between s and v => h = (s + v)
    
    public Rgba32 Shade(Scene scene, HitPoint hitPoint, Vector3 rayDirection, UInt16 depth = 0)
    {
        if (depth > MaxDepth) return new Rgba32(0, 0, 0);
        
        // Create a feeler ray to check for shadows with a small offset
        var epsilon = 0.001f;
        var feelerRay = new Ray(hitPoint.Point - epsilon * rayDirection, Vector3.Zero);
        
        // Calculate some vectors which are not dependent on the light
        var v = Vector3.Normalize(-rayDirection); // Camera direction
        var NdotV = Vector3.Dot(hitPoint.Normal, v);
        
        // Reset color values to ambient color
        _colorValues[0] = AmbientColor.X;
        _colorValues[1] = AmbientColor.Y;
        _colorValues[2] = AmbientColor.Z;
        
        var material = new StandardMaterial(); // TODO: Assign this to each specific object

        foreach (var light in scene.Lights)
        {
            // Compute a direction for the "feeler" ray to check for shadows
            feelerRay.Direction = light.GlobalPosition - hitPoint.Point;
            if (scene.IsInShadow(feelerRay))
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
            var DtimesG = NormalDistributionFunction(NdotH, material.SurfaceRoughness)
                          * Geometry(NdotH, NdotS, NdotV, HdotS);

            // For each RGB-channel, calculate the color value
            for (var i = 0; i < 3; i++)
            {
                // kD is dependant on our surface roughness
                var kD = material.SurfaceRoughness;
                var kS = 1 - kD;
            
                var specular = light.Color[i] * kS * FresnelFunction(NdotS, material.EtaFresnel[i]) * DtimesG / NdotV;
                var diffuse = light.Color[i] * kD * material.DiffuseColor[i];

                _colorValues[i] += specular + diffuse;
            }
        }
        
        // Clamp RGB values between 0-1
        System.Math.Clamp(_colorValues[0], 0.0f, 1.0f);
        System.Math.Clamp(_colorValues[1], 0.0f, 1.0f);
        System.Math.Clamp(_colorValues[2], 0.0f, 1.0f);

        return new Rgba32(_colorValues[0], _colorValues[1], _colorValues[2]);
    }

    /// <summary>
    /// Fresnel function which models the reflectance of the surface.
    /// </summary>
    /// <param name="c">Dot product of surface normal and s</param>
    /// <param name="eta">Index of refraction</param>
    /// <returns></returns>
    private float FresnelFunction(float c, float eta)
    {
        var g = MathF.Sqrt(MathF.Pow(eta, 2) + MathF.Pow(c, 2) - 1);

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