namespace RayTracingEngine.Material;

/// <summary>
/// This functions contains the Cook-Torrance functions. Moved out of the Shader class for better organization.
/// </summary>
public class CookTorrance
{
    /// <summary>
    /// Fresnel function which models the reflectance of the surface.
    /// </summary>
    /// <param name="c">Dot product of surface normal and s</param>
    /// <param name="eta">Index of refraction</param>
    /// <returns></returns>
    public static float FresnelFunction(float c, float eta)
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
    public static float NormalDistributionFunction(float cosDelta, float roughness)
    {
        var nom = MathF.Exp(-MathF.Pow(MathF.Tan(MathF.Acos(cosDelta)) / roughness, 2));
        return nom / (4 * MathF.Pow(roughness, 2) * MathF.Pow(cosDelta, 4));
    }

    public static float Geometry(float NdotH, float NdotS, float NdotV, float HdotS)
    {
        // Using Cook-Torrance model
        var gm = 2 * NdotH * NdotS / HdotS;
        var gs = 2 * NdotH * NdotV / HdotS;
        return MathF.Min(1, MathF.Min(gm, gs));
    }
}