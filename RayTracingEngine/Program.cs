using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Program
{
    public static void Main(string[] args)
    {
        var ray = new Ray(new Vector3(2,2,2),  new Vector3(0.5f, 0.5f, 0.5f));
        var obj = new Cube(Vector3.Zero, 1);
        
        // Check if the ray intersects with the sphere
        var hitInfo = obj.HitLocal(ray);
        
        if (hitInfo != null)
        {
            Console.WriteLine($"Hits: {hitInfo.Hits.Length}");
        }
        else
        {
            Console.WriteLine("No hit");
        }
    }
}