using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Material.Textures;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;
using RayTracingEngine.Textures;
using Plane = RayTracingEngine.Primitives.Plane;

namespace RayTracingEngine;

public class Program
{
    // Y -> Up, X -> Right, Z -> Negative is going further away from the screen
    public static void Main(string[] _)
    {
        const int width = 1920, height = 1080;

        // Create the lights
        var light1 = new Light(0.9f, Vector3.One, new Vector3(0, 1, 0));
        var light2 = new Light(1.0f, Vector3.One, new Vector3(4, -1, -5));
        
        // Create the objects
        var groundPlane = new Plane(new Vector3(0, -2, 0), new Vector3(0, 0, 0), new Vector3(10, 10, 10));
        var skyPlane = new Plane(new Vector3(0, 15, 0), new Vector3(-0.5235988f, 0, 0), new Vector3(10, 10, 10));
        var backPlane = new Plane(new Vector3(0, 0, 10), new Vector3(0.5235988f, 0, 0), new Vector3(10, 10, 10));
        var cube = new Cube(new Vector3(1, 0.5f, -2f), Vector3.Zero, Vector3.One);
        
        var surfaceNormalSphere = new Sphere(new Vector3(-2f, 0, -1.5f),  Vector3.Zero, Vector3.One);
        var glassSphere = new Sphere(new Vector3(-1f, 2f, 0),  Vector3.Zero, Vector3.One);
        var metallicSphere = new Sphere(new Vector3(-4f, 0, 0),  Vector3.Zero, Vector3.One);
        
        // Apply materials
        groundPlane.Material = new CheckerboardTexture();
        skyPlane.Material = new SkyMaterial();
        backPlane.Material = new WoodGrainTexture();
        cube.Material = new GoldMaterial();
        surfaceNormalSphere.Material = new SurfaceNormalTexture();
        glassSphere.Material = new MetalMaterial();
        metallicSphere.Material = new GlassMaterial();
        
        // Create the scene
        var scene = new Scene();
        scene.Lights.Add(light1);
        scene.Lights.Add(light2);
        scene.AddObject(groundPlane);
        scene.AddObject(skyPlane);
        scene.AddObject(backPlane);
        scene.AddObject(cube);
        scene.AddObject(surfaceNormalSphere);
        scene.AddObject(glassSphere);
        scene.AddObject(metallicSphere);
        
        var camera = new Camera(scene, new Vector3(5, 1, 5), new Vector3(0,0,0), width, height);
        
        // Create the window object
        var window = new ViewWindow(width, height, camera);
        window.Run();
    }
}