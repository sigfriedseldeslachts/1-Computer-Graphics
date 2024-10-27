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
        var sphere = new Sphere(new Vector3(5, 0, -5), 1);
        
        var camera = new Camera(new Vector3(0, 0, 10), width, height);
        camera.AddObject(sphere);
        
        // Create the window object
        var window = new ViewWindow(width, height, camera);
        
        window.Run();
    }
}