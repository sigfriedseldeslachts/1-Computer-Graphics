using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Cylinder : AObject
{
    private const float _height = 1.0f, _radius = 1.0f;
    
    public Cylinder(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
    {
        BuildTransformMatrix(position, rotation, scale);
    }

    public override float[] SimpleHitLocal(Ray ray)
    {
        var hits = new List<float>();

        // Check for intersection with the cylindrical surface
        var a = ray.Direction.X * ray.Direction.X + ray.Direction.Z * ray.Direction.Z;
        var b = 2 * (ray.Start.X * ray.Direction.X + ray.Start.Z * ray.Direction.Z);
        var c = ray.Start.X * ray.Start.X + ray.Start.Z * ray.Start.Z - _radius * _radius;

        var discriminant = b * b - 4 * a * c;
        if (discriminant >= 0)
        {
            var sqrtDiscriminant = MathF.Sqrt(discriminant);
            var t1 = (-b - sqrtDiscriminant) / (2 * a);
            var t2 = (-b + sqrtDiscriminant) / (2 * a);

            if (t1 > 0)
            {
                var y1 = ray.Start.Y + t1 * ray.Direction.Y;
                if (y1 >= 0 && y1 <= _height)
                {
                    hits.Add(t1);
                }
            }

            if (t2 > 0)
            {
                var y2 = ray.Start.Y + t2 * ray.Direction.Y;
                if (y2 >= 0 && y2 <= _height)
                {
                    hits.Add(t2);
                }
            }
        }

        // Check for intersection with the top and bottom faces
        if (MathF.Abs(ray.Direction.Y) > 1e-6)
        {
            var tTop = (_height - ray.Start.Y) / ray.Direction.Y;
            if (tTop > 0)
            {
                var xTop = ray.Start.X + tTop * ray.Direction.X;
                var zTop = ray.Start.Z + tTop * ray.Direction.Z;
                if (xTop * xTop + zTop * zTop <= _radius * _radius)
                {
                    hits.Add(tTop);
                }
            }

            var tBottom = -ray.Start.Y / ray.Direction.Y;
            if (tBottom > 0)
            {
                var xBottom = ray.Start.X + tBottom * ray.Direction.X;
                var zBottom = ray.Start.Z + tBottom * ray.Direction.Z;
                if (xBottom * xBottom + zBottom * zBottom <= _radius * _radius)
                {
                    hits.Add(tBottom);
                }
            }
        }

        return hits.ToArray();
    }

    public override HitPoint?[] HitLocal(Ray ray, Ray worldRay)
    {
        var hits = SimpleHitLocal(ray);
        if (hits.Length == 0) return [];
        
        // For each hit, calculate the hit point
        return hits.Select(hit =>
        {
            var hitPoint = ray.GetPoint(hit);
            var normal = Vector3.Normalize(hitPoint with { Y = 0 });
            
            // Check if the ray is entering or exiting the object
            var isEntering = Vector3.Dot(ray.Direction, normal) < 0;
            
            return new HitPoint
            {
                Object = this,
                HitTime = hit,
                Point = worldRay.GetPoint(hit),
                Normal = normal,
                IsEntering = isEntering
            };
        }).ToArray();
    }

    public override bool HasShadowHit(Ray ray)
    {
        var hit = SimpleHit(ray);
        return hit.Length > 0 && hit[0] > 0;
    }
}