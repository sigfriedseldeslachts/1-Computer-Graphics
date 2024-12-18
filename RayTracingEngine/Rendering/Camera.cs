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
    public Image<Rgba32> Image { get; set; }
    private readonly Shader Shader;
    
    private Vector3 _n;
    private Vector3 _u;
    private Vector3 _v;
    private int _nearPlaneDistance = 10;
    private float _H;
    private float _W;
    
    public Camera(Scene scene, Vector3 position, Vector3 lookingLocation, int width, int height)
    {
        Shader = new Shader(scene);
        Image = new Image<Rgba32>(width, height);
        Position = position;
        LookingLocation = lookingLocation;
        Width = width;
        Height = height;
        
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
        Image = new Image<Rgba32>(Width, Height);
        
        int rowPos, colPos;
        
        for (rowPos = 0; rowPos < Height; rowPos++)
        {
            for (colPos = 0; colPos < Width; colPos++)
            {
                ProcessPixel(rowPos, colPos);
            }
        }
        
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
        
        Image[colPos, rowPos] = Shader.Shade(ray);
    }
}