using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Cylinder : AObject
{
    private float _radius;
    
    public Cylinder(float radius, Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
    {
        // Check if radius is between 0 and 1
        if (radius is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be between 0 and 1");
        }
        
        _radius = radius;
        BuildTransformMatrix(position, rotation, scale);
    }

    public override float[] SimpleHitLocal(Ray ray)
    {
        var d = (_radius - 1) * ray.Direction.Z;
        var f = 1 + (_radius - 1) * ray.Start.Z;
        
        var a = MathF.Pow(ray.Direction.X, 2) + MathF.Pow(ray.Direction.Y, 2);
        var b = ray.Start.X * ray.Direction.X + ray.Start.Y * ray.Direction.Y - f*d;
        var c = MathF.Pow(ray.Start.X, 2) + MathF.Pow(ray.Start.Y, 2) - MathF.Pow(f, 2);
        
        var discriminant = MathF.Pow(b, 2) - a*c;
        if (discriminant < 0) return []; // No hit

        discriminant = MathF.Sqrt(discriminant);
        var t1 = (-b + discriminant) / a;
        var t2 = (-b - discriminant) / a;

        return [t1, t2];
    }

    public override HitPoint?[] HitLocal(Ray ray, Ray worldRay)
    {
        return [];
        
        var values = SimpleHitLocal(ray);
        if (values.Length == 0) return [];
        var hits = new HitPoint?[2];
        
        if (values[0] > 0.00001 || values[0] == 0)
        {
            var hit = ray.GetPoint(values[0]);
            hits[0] = new HitPoint
            {
                Object = this,
                HitTime = values[0],
                Point = worldRay.GetPoint(values[0]),
                Normal = Vector3.Normalize(hit - GlobalPosition),
                IsEntering = true
            };
        }

        if (values[1] > 0.00001)
        {
            var hit = ray.GetPoint(values[1]);
            hits[1] = new HitPoint
            {
                Object = this,
                HitTime = values[1],
                Point = worldRay.GetPoint(values[1]),
                Normal = Vector3.Normalize(hit - GlobalPosition),
                IsEntering = false
            };
        }

        return hits;
    }

    public override bool HasShadowHit(Ray ray)
    {
        var hit = SimpleHit(ray);
        return hit.Length > 0 && hit[0] > 0;
    }
}