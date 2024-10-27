using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Sphere : Object
{
    public Sphere(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
    {
        BuildTransformMatrix(position, rotation, scale);
    }
    
    public override HitInfo? HitLocal(Ray ray, bool transformBack = true)
    {
        if (ray.Direction == Vector3.Zero) return null;
        
        // Our sphere is x^2 + y^2 + z^2 - r^2 = 0. To check whether a ray intersects with the sphere,
        // we can substitute the ray vector with the x, y, z values and solve the quadratic equation.
        // Our ray is defined as Start + t * Direction. Therefore, the following must hold:
        // (Start + t * Direction)^2 - r^2 = 0
        // Expanding yields equation in form of ax^2 + bx + c = 0
        // https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection.html

        var a = ray.Direction.LengthSquared(); // Same as dot(ray.Direction, ray.Direction)
        var b = 2 * Vector3.Dot(ray.Direction, ray.Start);
        var c = ray.Start.LengthSquared() - 1; // Unit sphere radius is 1

        var discriminant = MathF.Pow(b, 2) - 4 * a * c;
        if (discriminant < 0.0f) return null;
        
        var sqrtDiscriminant = MathF.Sqrt(discriminant);
        var t0 = (-b - sqrtDiscriminant) / (2 * a);
        var t1 = (-b + sqrtDiscriminant) / (2 * a);
        
        var hits = new List<HitPoint>();
        
        if (t0 > 0.00001 || t0 == 0)
        {
            var hit = ray.GetPoint(t0);
            hits.Add(new HitPoint
            {
                HitTime = t0,
                Point = hit,
                Normal = GetNormal(hit),
                IsEntering = true
            });
        }

        if (t1 > 0.00001)
        {
            var hit = ray.GetPoint(t1);
            hits.Add(new HitPoint
            {
                HitTime = t1,
                Point = hit,
                Normal = GetNormal(hit),
                IsEntering = false
            });
        }
        
        if (hits.Count == 0) return null;
        return new HitInfo
        {
            Object = this,
            Hits = hits.ToArray()
        };
    }
    
    public Vector3 GetNormal(Vector3 point)
    {
        return Vector3.Normalize(point - GlobalPosition);
    }
}