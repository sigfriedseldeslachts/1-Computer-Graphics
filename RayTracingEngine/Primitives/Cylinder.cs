using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Cylinder : AObject
{
    public Cylinder(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
    {
        BuildTransformMatrix(position, rotation, scale);
    }

    public override float[] SimpleHitLocal(Ray ray)
    {
        throw new NotImplementedException();
    }

    public override HitPoint?[] HitLocal(Ray ray, Ray worldRay)
    {
        throw new NotImplementedException();
        //var a = 
    }

    public override bool HasShadowHit(Ray ray)
    {
        throw new NotImplementedException();
    }
}