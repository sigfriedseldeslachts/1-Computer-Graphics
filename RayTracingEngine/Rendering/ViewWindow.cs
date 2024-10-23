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
    public OpenGLRenderer GlRenderer { get; private set; }
    
    public ViewWindow(int width, int height)
    {
        Width = width;
        Height = height;
        
        CreateWindow();
    }

    private void CreateWindow()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(Width, Height);
        options.Title = "Ray Tracing Engine";
        
        Window = Silk.NET.Windowing.Window.Create(options);
        
        Window.Load += () =>
        {
            GlRenderer = new OpenGLRenderer(Window);
        };
        Window.Render += Draw;
        Window.Closing += () =>
        {
            GlRenderer.Dispose();
        };
    }
    
    private void Draw(double deltaTime)
    {
        GlRenderer.Draw();
    }
    
    public void Run()
    {
        Window.Run();
        Window.Dispose();
    }
}