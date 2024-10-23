using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Program
{
    public static void Main(string[] args)
    {
        int width = 1280, height = 720;
        
        // Create the window object
        var window = new ViewWindow(width, height);
        var camera = new Camera(Vector3.Zero, width, height);
        
        /*var obj = new Sphere(Vector3.Zero, 1);
        camera.AddObject(obj);
        
        camera.Render();*/
        
        window.Run();
    }
}