using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Sphere : AObject
{
    private List<HitPoint> _hits = [];
    
    public Sphere(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
    {
        BuildTransformMatrix(position, rotation, scale);
    }
    
    public override HitPoint[] HitLocal(Ray ray, bool transformBack = true)
    {
        if (ray.Direction == Vector3.Zero) return [];
        
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
        if (discriminant < 0.0f) return [];
        
        var sqrtDiscriminant = MathF.Sqrt(discriminant);
        var t0 = (-b - sqrtDiscriminant) / (2 * a);
        var t1 = (-b + sqrtDiscriminant) / (2 * a);
        
        _hits.Clear();
        
        if (t0 > 0.00001 || t0 == 0)
        {
            var hit = ray.GetPoint(t0);
            _hits.Add(new HitPoint
            {
                Object = this,
                HitTime = t0,
                Point = hit,
                Normal = Vector3.Normalize(hit - GlobalPosition),
                IsEntering = true
            });
        }

        if (t1 > 0.00001)
        {
            var hit = ray.GetPoint(t1);
            _hits.Add(new HitPoint
            {
                Object = this,
                HitTime = t1,
                Point = GetHitPoint(hit, transformBack),
                Normal = Vector3.Normalize(hit - GlobalPosition),
                IsEntering = false
            });
        }
        
        return _hits.ToArray();
    }
    
    public override Vector3 GetHitPoint(Vector3 point, bool transformBack)
    {
        return transformBack ? Vector3.Transform(point, TransposedInverseTransformMatrix) : point;
    }
}