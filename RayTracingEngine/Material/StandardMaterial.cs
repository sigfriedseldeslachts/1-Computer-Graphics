using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Material;

public class StandardMaterial
{
    public Vector4 AmbientColor { get; set; } = new(0.1f, 0.1f, 0.1f, 1.0f);
    public Vector4 DiffuseColor { get; set; } = new(0.5f, 0.5f, 0.5f, 1.0f);
    public Vector4 Fresnel { get; set; } = new(0.95f, 0.93f, 0.88f, 1.0f);
    public float SurfaceRoughness { get; set; } = 0.8f;
    public float ReflectionCoefficient { get; set; } = 0.1f;
    public float TransparencyCoefficient { get; set; } = 0.0f;
    public float RefractionIndex { get; set; } = 1.0f;

    public Vector4 EtaFresnel;
    
    public StandardMaterial()
    {
        CalculateEtaFresnel();
    }

    public virtual float[] GetDiffuseColor(HitPoint hit)
    {
        return
        [
            DiffuseColor.X,
            DiffuseColor.Y,
            DiffuseColor.Z,
            DiffuseColor.W
        ];
    }

    public virtual float[] GetAmbientColor(HitPoint hit)
    {
        return
        [
            AmbientColor.X,
            AmbientColor.Y,
            AmbientColor.Z,
            AmbientColor.W
        ];
    }

    /// <summary>
    /// Calculates the eta values for the Fresnel function (specular reflection). This is done to speed up the calculations
    /// </summary>
    private void CalculateEtaFresnel()
    {
        for (var i = 0; i < 3; i++)
        {
            EtaFresnel[i] = (1 + MathF.Sqrt(Fresnel[i])) / (1 - MathF.Sqrt(Fresnel[i]));
        }
    }
}