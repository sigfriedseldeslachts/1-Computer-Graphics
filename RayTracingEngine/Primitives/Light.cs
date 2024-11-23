using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Light : AObject
{
    public float Intensity { get; set; }
    public Vector3 Direction { get; set; }
    
    public Light(float intensity, Vector3 lightDirection, Vector3 position) : base(position, Vector3.Zero, Vector3.Zero)
    {
        Intensity = intensity;
        Direction = lightDirection;
        BuildTransformMatrix(position, Vector3.Zero, Vector3.Zero);
    }

    public override HitPoint[] HitLocal(Ray ray, bool transformBack = true)
    {
        throw new NotImplementedException();
    }

    public override Vector3 GetHitPoint(Vector3 point, bool transformBack)
    {
        throw new NotImplementedException();
    }
}