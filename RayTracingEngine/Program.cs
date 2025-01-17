using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;
using RayTracingEngine.Textures;
using Plane = RayTracingEngine.Primitives.Plane;

namespace RayTracingEngine;

public class Program
{
    // Y -> Up, X -> Right, Z -> Negative is going further away from the screen
    public static void Main(string[] args)
    {
        const int width = 1920, height = 1080;

        // Create the lights
        var light = new Light(1.0f, Vector3.One, new Vector3(0, 1, 0));
        
        // Create the objects
        var groundPlane = new Plane(new Vector3(0, -5, 0), new Vector3(0, 0, 0), new Vector3(10, 10, 10));
        var skyPlane = new Plane(new Vector3(0, 15, 0), new Vector3(-0.5235988f, 0, 0), new Vector3(10, 10, 10));
        var sphere = new Sphere(new Vector3(5, 2, -4),  Vector3.Zero, Vector3.One);
        var cube = new Cube(new Vector3(0,0,-5),  Vector3.Zero, Vector3.One);
        
        // Apply materials
        groundPlane.Material = new CheckerboardTexture();
        skyPlane.Material = new SkyMaterial();
        cube.Material = new GoldMaterial();
        sphere.Material = new GlassMaterial();
        
        // Create the scene
        var scene = new Scene();
        scene.Lights.Add(light);
        scene.AddObject(groundPlane);
        scene.AddObject(skyPlane);
        scene.AddObject(cube);
        scene.AddObject(sphere);
        
        var camera = new Camera(scene, new Vector3(0, 5, 10), new Vector3(0,0,0), width, height);
        
        // Create the window object
        var window = new ViewWindow(width, height, camera);
        window.Run();
    }
}