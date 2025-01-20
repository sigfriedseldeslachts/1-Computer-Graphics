using System.Numerics;
using RayTracingEngine.Math;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Material.Textures;

public class MarbleTexture : StandardMaterial
{
    public MarbleTexture()
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
        var noise = new Noise().GetNoise(10.0f, hit.Point);
        return
        [
            noise,
            noise,
            noise,
            DiffuseColor.W
        ];
    }

    public override float[] GetAmbientColor(HitPoint hit)
    {
        return
        [
            AmbientColor[0],
            AmbientColor[1],
            AmbientColor[2],
            AmbientColor.W
        ];
    }
}