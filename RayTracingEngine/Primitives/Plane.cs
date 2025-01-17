using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Plane : AObject
{
    public Plane(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
    {
        BuildTransformMatrix(position, rotation, scale);
    }

    public override float[] SimpleHitLocal(Ray ray)
    {
        var t = -Vector3.Dot(ray.Start, Vector3.UnitY) / Vector3.Dot(ray.Direction, Vector3.UnitY);
        if (t < 0.0001) return [];

        var hitPoint = ray.GetPoint(t);
        if (MathF.Abs(hitPoint.X) > Scale.X / 2 || MathF.Abs(hitPoint.Z) > Scale.Z / 2) return [];

        return [t];
    }

    public override bool HasShadowHit(Ray ray)
    {
        var hit = SimpleHit(ray);
        if (hit.Length == 0) return false;
        return hit[0] < 1; // If the hit is closer than 1 unit, it is in shadow
    }

    public override HitPoint?[] HitLocal(Ray ray, Ray worldRay)
    {
        var values = SimpleHitLocal(ray);
        if (values.Length == 0) return [];

        return
        [
            new HitPoint
            {
                Object = this,
                HitTime = values[0],
                Point = worldRay.GetPoint(values[0]),
                Normal = Vector3.UnitY,
                IsEntering = true
            }
        ];
    }
}