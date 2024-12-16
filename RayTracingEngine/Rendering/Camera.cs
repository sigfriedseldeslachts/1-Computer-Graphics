using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.Rendering;

public class Camera(Scene scene, Vector3 position, int width, int height)
{
    public Vector3 Position { get; set; } = position;
    public float NearPlaneDistance { get; set; } = 5;
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;

    public Image<Rgba32> Image { get; set; } = new(width, height);
    
    public readonly List<HitPoint> Hits = [];
    
    public readonly Shader Material = new();

    public void Render()
    {
        Image = new Image<Rgba32>(Width, Height);

        int rowPos, colPos;
        var ray = new Ray(Position, Vector3.Zero);
        
        var H = NearPlaneDistance * MathF.Tan(MathF.PI / 4);
        var W = H * ((float) Width / Height); // H * Aspect ratio
        
        for (rowPos = 0; rowPos < Height; rowPos++)
        {
            for (colPos = 0; colPos < Width; colPos++)
            {
                // For each ray we clear the hits
                Hits.Clear();
                
                // We want the center to be X = 0, Y = 0. Thus, we need to shift everything by half the width and height
                var x = W * (2*colPos / (float) Width - 1);
                var y = H * (2*rowPos / (float) Height - 1);

                // Now we need to create a ray direction
                ray.Direction = Vector3.Normalize(new Vector3(x, y, -position.Z));
                
                // Draw the pixel
                DrawPixel(colPos, rowPos, scene.GetBestHit(ray), ray.Direction);
            }
        }
        
        Console.WriteLine("Frame rendered");
    }

    /// <summary>
    /// Draws a pixel at coordinates x, y with the given hits
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="hitPoint"></param>
    /// <param name="rayDirection"></param>
    private void DrawPixel(int x, int y, HitPoint? hitPoint, Vector3 rayDirection)
    {
        if (hitPoint == null)
        {
            Image[x, y] = new Rgba32(0, 0, 0);
            return;
        }
        
        Image[x, y] = Material.Shade(scene, hitPoint, rayDirection);
    }
}