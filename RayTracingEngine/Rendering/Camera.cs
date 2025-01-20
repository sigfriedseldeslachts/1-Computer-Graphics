using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Math;
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
    private Shader _shader;
    
    private Vector3 _n;
    private Vector3 _u;
    private Vector3 _v;
    private int _nearPlaneDistance = 10;
    private float _H;
    private float _W;
    private int _samplesPerPixel = 1;
    private float _pixelSamplesScale = 1.0f / 1.0f;
    
    private readonly ParallelOptions _parallelOptions = new() { MaxDegreeOfParallelism = 6 };

    public Camera(int width, int height)
    {
        Width = width;
        Height = height;
        _imageBuffer0 = new Image<Rgba32>(Width, Height);
        _imageBuffer1 = new Image<Rgba32>(Width, Height);
    }

    public void SetCamera(Scene scene, Vector3 position, Vector3 lookingLocation)
    {
        Position = position;
        LookingLocation = lookingLocation;
        _shader = new Shader(scene);
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
        // Do anti-aliasing
        var color = new float[4];
        
        var ray = new Ray(Position, Vector3.Zero);

        // We want the center to be X = 0, Y = 0. Thus, we need to shift everything by half the width and height
        var x = _W * (2*colPos / (float) Width - 1);
        var y = _H * (2*rowPos / (float) Height - 1);
        
        for (var sample = 0; sample < _samplesPerPixel; sample++)
        {
            // Set the ray direction with a random offset
            //var randomX = Utils.RandomFloat(-0.1f, 0.1f);
            //var randomY = Utils.RandomFloat(-0.1f, 0.1f);
            ray.Direction = Vector3.Normalize(_n * _nearPlaneDistance + (x) * _u + (y) * _v);
            
            var shadeColor = _shader.Shade(ray);
            color[0] += shadeColor[0];
            color[1] += shadeColor[1];
            color[2] += shadeColor[2];
            color[3] += shadeColor[3];
        }

        // Clamp and scale the values
        color = Utils.ClampRgbaValues(color);
        color[0] *= _pixelSamplesScale;
        color[1] *= _pixelSamplesScale;
        color[2] *= _pixelSamplesScale;
        color[3] *= _pixelSamplesScale;
        
        // Set the pixel color in the active buffer
        if (_activeBuffer == 0)
        {
            _imageBuffer0[colPos, rowPos] = new Rgba32(color[0], color[1], color[2]);
        }
        else
        {
            _imageBuffer1[colPos, rowPos] = new Rgba32(color[0], color[1], color[2]);
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