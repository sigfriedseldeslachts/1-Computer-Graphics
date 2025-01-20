using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Program
{
    // Y -> Up, X -> Right, Z -> Negative is going further away from the screen
    public static void Main(string[] _)
    {
        const int width = 1280, height = 720;

        var camera = new Camera(width, height);
        var loader = new SceneLoader(camera);
        
        loader.AllInOneScene();
        
        // Create the window object
        var window = new ViewWindow(width, height, camera);
        window.Run();
    }
}