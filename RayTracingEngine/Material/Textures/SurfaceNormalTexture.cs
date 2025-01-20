using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Textures;

// Texture that shows the surface normals
// From: https://raytracing.github.io/books/RayTracingInOneWeekend.html#surfacenormalsandmultipleobjects/shadingwithsurfacenormals
public class SurfaceNormalTexture : StandardMaterial
{
    public SurfaceNormalTexture()
    {
        AmbientColor = new Vector4(0.1f, 0.1f, 0.1f, 1);
        DiffuseColor = new Vector4(0.1f, 0.1f, 0.1f, 1);
        RefractionIndex = 0;
        ReflectionCoefficient = 0;
        TransparencyCoefficient = 0;
    }
    
    public override float[] GetDiffuseColor(HitPoint hit)
    {
        return [
            (hit.Normal.X + 1) / 2,
            (hit.Normal.Y + 1) / 2,
            (hit.Normal.Z + 1) / 2,
            1
        ];
    }
    
    public override float[] GetAmbientColor(HitPoint hit)
    {
        return [
            (hit.Normal.X + 1) / 4,
            (hit.Normal.Y + 1) / 4,
            (hit.Normal.Z + 1) / 4,
            1
        ];
    }
}