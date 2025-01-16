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
    private Thread _renderThread;
    private bool _stopRendering = false;
    
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
        _stopRendering = true;
        Window.Dispose();
    }

    private void CreateWindow()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(Width, Height);
        options.Title = "Ray Tracing Engine";
        
        // Start an extra thread to render the camera image
        _renderThread = new Thread(() =>
        {
            while (!_stopRendering)
            {
                Camera.Render();
                _stopRendering = true; // Temp: only render once!
            }
        });
        
        Window = Silk.NET.Windowing.Window.Create(options);
        Window.Load += () =>
        {
            GlRenderer = new OpenGlRenderer(Window);
            _renderThread.Start();
        };
        Window.Update += (deltaTime) =>
        {
            
        };
        Window.Render += (deltaTime) =>
        {
            GlRenderer.SetTextureFromBitmap(Camera.GetActiveImage());
            GlRenderer.Draw();
        };
        Window.Closing += () =>
        {
            GlRenderer.Dispose();
        };
    }
}