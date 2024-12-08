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
        
        var sphere = new Sphere(new Vector3(-3,-3,-3),  Vector3.Zero, Vector3.One);
        var cube = new Cube(new Vector3(3, 3, 3),  Vector3.Zero, Vector3.One);
        
        var light = new Light(1.0f, new Vector3(-0.45f, -1.5f, 0.5f), new Vector3(7, 7, 0));
        var light2 = new Light(1.0f, new Vector3(10, 5, 10), new Vector3(0, 0, 0));

        var scene = new Scene();
        scene.AddObject(encompassingCube);
        scene.AddObject(cube);
        //scene.AddObject(sphere);
        scene.Lights.Add(light);
        //scene.Lights.Add(light2);
        
        var camera = new Camera(scene, new Vector3(0, 0, 50), width, height);
        
        // Create the window object
        var window = new ViewWindow(width, height, camera);
        window.Run();
    }
}