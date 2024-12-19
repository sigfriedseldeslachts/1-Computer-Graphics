using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Program
{
    public static void Main(string[] args)
    {
        const int width = 1280, height = 720;

        // Y -> Up, X -> Right, Z -> Negative is going further away from the screen
  
        var encompassingCube = new Cube(Vector3.Zero, Vector3.Zero, new Vector3(100,100,100));
        
        var sphere = new Sphere(new Vector3(5, 2, -4),  Vector3.Zero, Vector3.One);
        var cube = new Cube(new Vector3(0,0,-5),  Vector3.Zero, Vector3.One);
        sphere.Material = new GoldMaterial();
        
        var light = new Light(1.0f, Vector3.One, new Vector3(-2, 0, 15));

        var scene = new Scene();
        scene.AddObject(encompassingCube);
        scene.AddObject(cube);
        scene.AddObject(sphere);
        scene.Lights.Add(light);
        //scene.Lights.Add(light2);
        
        var camera = new Camera(scene, new Vector3(10, 0, 20), new Vector3(0,0,0), width, height);
        
        // Create the window object
        var window = new ViewWindow(width, height, camera);
        window.Run();
    }
}