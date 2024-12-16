using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Light : AObject
{
    public float Intensity { get; set; }
    public Vector3 Color { get; set; } = Vector3.One;
    
    public Light(float intensity, Vector3 position) : base(position, Vector3.Zero, Vector3.Zero)
    {
        Intensity = intensity;
        BuildTransformMatrix(position, Vector3.Zero, Vector3.Zero);
    }
    
    public override float[] SimpleHitLocal(Ray ray)
    {
        throw new NotImplementedException();
    }

    public override HitPoint[] HitLocal(Ray ray, Ray worldRay)
    {
        throw new NotImplementedException();
    }

    public override bool HasShadowHit(Ray ray)
    {
        throw new NotImplementedException();
    }
}