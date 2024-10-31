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
        var sphere = new Sphere(new Vector3(5, 0, 0),  new Vector3(0, 0, float.DegreesToRadians(45)), new Vector3(1, 4, 4));
        var cube = new Cube(Vector3.Zero, Vector3.Zero, Vector3.One);
        
        var camera = new Camera(new Vector3(0, 0, 100), width, height);
        camera.AddObject(sphere);
        camera.AddObject(cube);

        // Create the window object
        var window = new ViewWindow(width, height, camera);
        window.Run();
    }
}