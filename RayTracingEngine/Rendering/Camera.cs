using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.Rendering;

public class Camera
{
    public Vector3 Position { get; set; }
    public Vector3 LookingLocation { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Fov { get; set; } = 50;
    
    private ushort _activeBuffer;
    private Image<Rgba32> _imageBuffer0;
    private Image<Rgba32> _imageBuffer1;
    private readonly Shader _shader;
    
    private Vector3 _n;
    private Vector3 _u;
    private Vector3 _v;
    private int _nearPlaneDistance = 10;
    private float _H;
    private float _W;
    
    private readonly ParallelOptions _parallelOptions = new() { MaxDegreeOfParallelism = 6 };
    
    public Camera(Scene scene, Vector3 position, Vector3 lookingLocation, int width, int height)
    {
        _shader = new Shader(scene);
        Position = position;
        LookingLocation = lookingLocation;
        Width = width;
        Height = height;
        
        _imageBuffer0 = new Image<Rgba32>(Width, Height);
        _imageBuffer1 = new Image<Rgba32>(Width, Height);
        
        UpdateVectors();
    }

    public void UpdateVectors()
    {
        _n = Vector3.Normalize(LookingLocation - Position); // Camera direction
        _u = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, _n)); // Camera "right" vector
        _v = Vector3.Normalize(Vector3.Cross(_n, _u)); // Camera "up" vector
        
        _H = _nearPlaneDistance * MathF.Tan(Fov / 2.0f * (MathF.PI / 180));
        _W = _H * ((float) Width / Height); // H * Aspect ratio
    }

    public void Render()
    {
        Parallel.For(0, Height, _parallelOptions, rowPos =>
        {
            for (var colPos = 0; colPos < Width; colPos++)
            {
                ProcessPixel(rowPos, colPos);
            }
        });
        
        //_activeBuffer = _activeBuffer == 0 ? (ushort) 1 : (ushort) 0;
        Console.WriteLine("Frame rendered");
    }
    
    private void ProcessPixel(int rowPos, int colPos)
    {
        var ray = new Ray(Position, Vector3.Zero);

        // We want the center to be X = 0, Y = 0. Thus, we need to shift everything by half the width and height
        var x = _W * (2*colPos / (float) Width - 1);
        var y = _H * (2*rowPos / (float) Height - 1);
        
        // Now we need to create a ray direction
        ray.Direction = Vector3.Normalize(_n * _nearPlaneDistance + x * _u + y * _v);
        
        var colors = _shader.Shade(ray);
        
        // Set the pixel color in the active buffer
        if (_activeBuffer == 0)
        {
            _imageBuffer0[colPos, rowPos] = new Rgba32(colors[0], colors[1], colors[2]);
        }
        else
        {
            _imageBuffer1[colPos, rowPos] = new Rgba32(colors[0], colors[1], colors[2]);
        }
    }
    
    /// <summary>
    /// Returns the image that is currently not being rendered.
    /// </summary>
    /// <returns></returns>
    public Image<Rgba32> GetActiveImage()
    {
        return _imageBuffer0;
        //return _activeBuffer == 0 ? _imageBuffer1 : _imageBuffer0;
    }
}