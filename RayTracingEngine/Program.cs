using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Program
{
    public static void Main(string[] args)
    {
        int width = 1280, height = 720;
  
        var encompassingCube = new Cube(Vector3.Zero, Vector3.Zero, new Vector3(100,100,100));
        var sphere = new Sphere(Vector3.Zero,  Vector3.Zero, Vector3.One);
        var light = new Light(1.0f, new Vector3(10, 5, 10), new Vector3(0, 0, 0));
        
        var camera = new Camera(new Vector3(-5, 0, 50), width, height);
        camera.AddObject(sphere);
        camera.AddObject(encompassingCube);
        camera.Lights.Add(light);

        // Create the window object
        var window = new ViewWindow(width, height, camera);
        window.Run();
    }
}