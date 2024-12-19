using System.Numerics;

namespace RayTracingEngine.Material;

public class StandardMaterial
{
    public Vector4 AmbientColor { get; set; } = new(0.1f, 0.1f, 0.1f, 1.0f);
    public Vector4 DiffuseColor { get; set; } = new(0.5f, 0.5f, 0.5f, 1.0f);
    public Vector4 Fresnel { get; set; } = new(0.95f, 0.93f, 0.88f, 1.0f);
    public float SurfaceRoughness { get; set; } = 0.5f;
    
    public float ReflectionCoefficient { get; set; } = 0.1f;
    public float RefractionCoefficient { get; set; } = 0.0f;

    public Vector4 EtaFresnel;
    
    public StandardMaterial()
    {
        CalculateEtaFresnel();
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