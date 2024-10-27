using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Program
{
    public static void Main(string[] args)
    {
        int width = 1280, height = 720;
        
        // Create a sphere at the center of the screen
        var sphere = new Sphere(new Vector3(10, 20, 5),  Vector3.Zero, new Vector3(1, 4, 4));
        
        var camera = new Camera(new Vector3(0, 0, 10), width, height);
        camera.AddObject(sphere);
        //camera.Render();

        var start = new Vector3(10, 20, 5);
        var direction = new Vector3(-8, -12, 4);
        var ray = new Ray(start, direction);
        var hit = sphere.Hit(ray);
        if (hit != null)
        {
            foreach (var hitInfo in hit.Hits)
            {
                Console.WriteLine(hitInfo.Point);
            }
        }
        else
        {
            Console.WriteLine("No hit");
        }

        // Create the window object
        //var window = new ViewWindow(width, height, camera);
        //window.Run();
    }
}