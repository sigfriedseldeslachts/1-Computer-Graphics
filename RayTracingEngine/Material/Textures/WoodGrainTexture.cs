using System.Numerics;
using RayTracingEngine.Math;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Material.Textures;

public class WoodGrainTexture : StandardMaterial
{
    public Vector4 Diffuse2Color { get; set; } = new Vector4(0.31f, 0.12f, 0.12f, 1.0f);
    
    
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
            DiffuseColor[0],
            DiffuseColor[1],
            DiffuseColor[2],
            DiffuseColor.W
        ];
    }

    public override float[] GetAmbientColor(HitPoint hit)
    {
        var woodGrainValue = Rings(hit.Point);
        return
        [
            AmbientColor[0],
            AmbientColor[1],
            AmbientColor[2],
            AmbientColor.W
        ];
    }

    private float Rings(Vector3 point)
    {
        /*var radius = MathF.Sqrt(MathF.Pow(point.X, 2) + MathF.Pow(point.Y, 2));
        var theta = MathF.Atan2(point.Y, point.X);
        var value = radius / Thickness + MathF.Sin(theta / RingWobble);
        value /= 100; // Normalize to 0-1 
        return value % 2;*/

        return (int) MathF.Sqrt(MathF.Pow(point.X, 2) + MathF.Pow(point.Y, 2)) % 2;
    }
}