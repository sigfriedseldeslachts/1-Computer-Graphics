using RayTracingEngine.OpenGL;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace RayTracingEngine.Rendering;

public class ViewWindow
{
    public int Width { get; }
    public int Height { get; }
    public IWindow Window { get; private set; }
    public OpenGlRenderer GlRenderer { get; private set; }
    public Camera Camera { get; private set; }
    
    public ViewWindow(int width, int height, Camera camera)
    {
        Width = width;
        Height = height;
        Camera = camera;
        
        CreateWindow();
    }
    
    public void Run()
    {
        Window.Run();
        Window.Dispose();
    }

    private void CreateWindow()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(Width, Height);
        options.Title = "Ray Tracing Engine";
        
        Window = Silk.NET.Windowing.Window.Create(options);
        
        Window.Load += () =>
        {
            GlRenderer = new OpenGlRenderer(Window);
        };
        Window.Update += (deltaTime) =>
        {
            Camera.Render();
        };
        Window.Render += (deltaTime) =>
        {
            GlRenderer.SetTextureFromBitmap(Camera.Image);
            Draw();
        };
        Window.Closing += () =>
        {
            GlRenderer.Dispose();
        };
    }
    
    private void Draw()
    {
        GlRenderer.Draw();
    }
}