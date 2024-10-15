using System.Numerics;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Sphere : Object
{
    private float _x, _y, _z, _radius;
    
    public Sphere(float x, float y, float z, float _radius)
    {
        // X Y Z Must be smaller than 1
        if (x > 1 || y > 1 || z > 1)
        {
            throw new System.Exception("X Y Z Must be smaller than 1");
        }
        
        _x = x;
        _y = y;
        _z = z;
        this._radius = _radius;
    }
    
    public override HitInfo? HitLocal(Ray ray)
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
        var c = ray.Start.LengthSquared() - MathF.Pow(_radius, 2);

        var discriminant = MathF.Pow(b, 2) - 4 * a * c;
        Console.WriteLine(discriminant);
        if (discriminant < 0.0f) return null;
        
        return new HitInfo
        {
            Object = this,
            //Point = ray.Origin,
            //Normal = ray.Origin,
            //Distance = 0
        };
    }
}