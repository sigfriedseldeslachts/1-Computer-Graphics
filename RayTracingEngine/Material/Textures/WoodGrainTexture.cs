using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Material.Textures;

public class WoodGrainTexture : StandardMaterial
{
    public float Thickness { get; set; } = 1;
    public float RingWobble { get; set; } = 5;
    
    public WoodGrainTexture()
    {
        DiffuseColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
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
            DiffuseColor[0] + woodGrainValue,
            DiffuseColor[1] + woodGrainValue,
            DiffuseColor[2] + woodGrainValue,
            DiffuseColor.W
        ];
    }

    public override float[] GetAmbientColor(HitPoint hit)
    {
        var woodGrainValue = Rings(hit.Point);
        return
        [
            AmbientColor[0] + woodGrainValue,
            AmbientColor[1] + woodGrainValue,
            AmbientColor[2] + woodGrainValue,
            AmbientColor.W
        ];
    }

    private float Rings(Vector3 point)
    {
        var radius = MathF.Sqrt(MathF.Pow(point.X, 2) + MathF.Pow(point.Y, 2));
        var theta = MathF.Atan2(point.Y, point.X);
        var value = radius / Thickness + MathF.Sin(theta / RingWobble);
        return value % 2;
    }
}