using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Material.Textures;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;
using RayTracingEngine.Textures;
using Plane = RayTracingEngine.Primitives.Plane;

namespace RayTracingEngine;

public class SceneLoader
{
    public Camera Camera { get; }
    
    public SceneLoader(Camera camera)
    {
        Camera = camera;
    }

    public void RefractionTest()
    {
        var light = new Light(0.9f, Vector3.One, new Vector3(0, 3, 1));
        
        var groundPlane = new Plane(new Vector3(0, -2, 0), new Vector3(0, 0, 0), new Vector3(10, 10, 10));
        var glassSphere = new Sphere(new Vector3(0, 0, 0),  Vector3.Zero, Vector3.One);
        
        groundPlane.Material = new CheckerboardTexture();
        glassSphere.Material = new GlassMaterial();
        
        var scene = new Scene();
        scene.Lights.Add(light);
        scene.AddObject(groundPlane);
        scene.AddObject(glassSphere);
        
        Camera.SetCamera(scene, new Vector3(0,0,10), new Vector3(0,0,0));
    }

    public void MirrorScene()
    {
        var light = new Light(0.9f, Vector3.One, new Vector3(0, 3, 1));
        
        var groundPlane = new Plane(new Vector3(0, -2, 0), new Vector3(0, 0, 0), new Vector3(10, 10, 10));
        groundPlane.Material = new CheckerboardTexture();
        
        // Create a rectangular mirror
        var mirror = new Cube(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(5, 1, 1));
        mirror.Material = new MetalMaterial();
        
        // Place a sphere in front of the mirror
        var sphere = new Sphere(new Vector3(2, 0, 3), Vector3.Zero, Vector3.One);
        sphere.Material = new GoldMaterial();
        
        // Place a cube behind the mirror
        var cube = new Cube(new Vector3(-4, 1, -3), Vector3.Zero, new Vector3(1, 1, 1));
        cube.Material = new SkyMaterial();
        
        var scene = new Scene();
        scene.Lights.Add(light);
        scene.AddObject(groundPlane);
        scene.AddObject(mirror);
        scene.AddObject(sphere);
        scene.AddObject(cube);
        
        Camera.SetCamera(scene, new Vector3(0,1,10), new Vector3(0, 0, 0));
    }

    public void AllInOneScene()
    {
        // Create the lights
        var light1 = new Light(0.9f, Vector3.One, new Vector3(0, 1, 0));
        var light2 = new Light(0.8f, Vector3.One, new Vector3(4, 6, -5));
        
        // Create the objects
        var groundPlane = new Plane(new Vector3(0, -2, 0), new Vector3(0, 0, 0), new Vector3(10, 10, 10));
        var skyPlane = new Plane(new Vector3(0, 15, 0), new Vector3(-0.5235988f, 0, 0), new Vector3(10, 10, 10));
        var backPlane = new Plane(new Vector3(0, 0, 10), new Vector3(0.5235988f, 0, 0), new Vector3(10, 10, 10));
        var cube = new Cube(new Vector3(1, 0.5f, -2f), Vector3.Zero, Vector3.One);
        
        var metalSphere = new Sphere(new Vector3(-2f, 0, -1.5f),  Vector3.Zero, Vector3.One);
        var surfNormSphere = new Sphere(new Vector3(-1f, 2f, 0),  Vector3.Zero, Vector3.One);
        var glassSphere = new Sphere(new Vector3(-4f, 0, 0),  Vector3.Zero, Vector3.One);
        var sphereBehindGlass = new Sphere(new Vector3(-7f, 0, -1f),  Vector3.Zero, Vector3.One);
        
        // Apply materials
        groundPlane.Material = new CheckerboardTexture();
        skyPlane.Material = new SkyMaterial();
        backPlane.Material = new WoodGrainTexture();
        cube.Material = new GoldMaterial();
        metalSphere.Material = new MetalMaterial();
        surfNormSphere.Material = new SurfaceNormalTexture();
        glassSphere.Material = new GlassMaterial();
        sphereBehindGlass.Material = new EmeraldMaterial();
        
        // Create the scene
        var scene = new Scene();
        scene.Lights.Add(light1);
        scene.Lights.Add(light2);
        scene.AddObject(groundPlane);
        scene.AddObject(skyPlane);
        scene.AddObject(backPlane);
        scene.AddObject(cube);
        scene.AddObject(metalSphere);
        scene.AddObject(surfNormSphere);
        scene.AddObject(glassSphere);
        scene.AddObject(sphereBehindGlass);
        
        // Set camera
        Camera.SetCamera(scene, new Vector3(5, 1, 5), new Vector3(0, 0, 0));
    }
    
    
}