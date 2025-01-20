using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Textures;

public class CheckerboardTexture : StandardMaterial
{
    private const float Threshold = 0.0001f; // Prevents floating point issues
    
    public Vector3 CheckerboardScale { get; set; } = new(0.5f, 0.5f, 0.5f);
    public Vector4 SecondColorDiffuse = new Vector4(0.1f, 0.1f, 0.1f, 1);
    public Vector4 SecondColorAmbient = new Vector4(0, 0, 0, 1);

    public override float[] GetDiffuseColor(HitPoint hit)
    {
        return GetCheckerboardPatternForCoords(hit.Point) ? base.GetDiffuseColor(null!) : [
            SecondColorDiffuse.X, SecondColorDiffuse.Y, SecondColorDiffuse.Z, SecondColorDiffuse.W
        ];
    }

    public override float[] GetAmbientColor(HitPoint hit)
    {
        return GetCheckerboardPatternForCoords(hit.Point) ? base.GetAmbientColor(null!) : [
            SecondColorAmbient.X, SecondColorAmbient.Y, SecondColorAmbient.Z, SecondColorAmbient.W
        ];
    }

    private bool GetCheckerboardPatternForCoords(Vector3 point)
    {
        var x = (int) MathF.Floor((point.X + Threshold) * CheckerboardScale.X);
        var y = (int) MathF.Floor((point.Y + Threshold) * CheckerboardScale.Y);
        var z = (int) MathF.Floor((point.Z + Threshold) * CheckerboardScale.Z);
        return (x + y + z) % 2 == 0;
    }
}