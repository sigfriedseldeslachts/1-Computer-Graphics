using System.Numerics;
using RayTracingEngine.Math;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Material.Textures;

public class WoodGrainTexture : StandardMaterial
{
    public Vector4 Diffuse2Color { get; set; } = new Vector4(0.31f, 0.12f, 0.12f, 1.0f);
    public float M = 0.2f; // Ring thickness
    public float N = 6.5f; // Ring wobble
    
    public WoodGrainTexture()
    {
        DiffuseColor = new Vector4(0.31f, 0.12f, 0.12f, 1.0f);
        AmbientColor = new Vector4(0.1f, 0.1f, 0.1f, 1.0f);
        Fresnel = new Vector4(0.95f, 0.93f, 0.88f, 1.0f);
        SurfaceRoughness = 0.5f;
        ReflectionCoefficient = 0.1f;
        TransparencyCoefficient = 0.0f;
        RefractionIndex = 1.0f;
    }
    
    public override float[] GetDiffuseColor(HitPoint hit)
    {
        var woodGrainValue = Rings(hit.Point);
        return
        [
            DiffuseColor[0] + Diffuse2Color[0] * woodGrainValue,
            DiffuseColor[1] + Diffuse2Color[1] * woodGrainValue,
            DiffuseColor[2] + Diffuse2Color[2] * woodGrainValue,
            DiffuseColor.W
        ];
    }

    public override float[] GetAmbientColor(HitPoint hit)
    {
        var woodGrainValue = Rings(hit.Point);
        return
        [
            AmbientColor[0] + Diffuse2Color[0] * woodGrainValue,
            AmbientColor[1] + Diffuse2Color[1] * woodGrainValue,
            AmbientColor[2] + Diffuse2Color[2] * woodGrainValue,
            AmbientColor.W
        ];
    }

    private float Rings(Vector3 point)
    {
        var radius = MathF.Sqrt(MathF.Pow(point.X, 2) + MathF.Pow(point.Y, 2));
        
        // Theta is the azimuthal angle about the z-axis
        var theta = MathF.Atan2(point.Y, point.X);
        var value = radius / M + MathF.Sin(theta / N + point.Z);
        return value % 2;
    }
}