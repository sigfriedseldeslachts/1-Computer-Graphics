using System.Numerics;
using RayTracingEngine.Math;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Material.Textures;

public class RandomNoise : StandardMaterial
{
    public RandomNoise()
    { }
    
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
        var noise = new Noise().GetNoise(10.0f, hit.Point);
        return
        [
            noise * 0.5f,
            noise * 0.5f,
            noise * 0.5f,
            AmbientColor.W
        ];
    }
}