using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Program
{
    public static void Main(string[] args)
    {
        // Create a ray from the origin to the point (1, 1, 1)
        var ray = new Ray(new Vector3(0, 5, 1.0f), new Vector3(0, -5, 1f));
        
        // Create a sphere at (0, 0, 0) with a radius of 1
        var sphere = new Sphere(0, 0, 0, 1);
        
        // Check if the ray intersects with the sphere
        var hitInfo = sphere.Hit(ray);
        
        if (hitInfo != null)
        {
            Console.WriteLine($"Hit at time: {hitInfo.HitTime}");
        }
        else
        {
            Console.WriteLine("No hit");
        }
    }
}