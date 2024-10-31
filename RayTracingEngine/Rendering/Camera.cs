using System.Numerics;
using RayTracingEngine.Material;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.Rendering;

public class Camera(Vector3 position, int width, int height)
{
    public Vector3 Position { get; set; } = position;
    public float NearPlaneDistance { get; set; } = 5;
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
    
    public Image<Rgba32> Image { get; set; } = new(width, height);
    
    public readonly List<IRayTraceable> Objects = new List<IRayTraceable>();
    public readonly List<HitInfo> Hits = new List<HitInfo>();
    
    public readonly AMaterial Material = new();
    
    public readonly Vector3 LightDirection = new(20, 20, 20);

    public void Render()
    {
        Image = new Image<Rgba32>(Width, Height);

        int rowPos, colPos;
        HitInfo? hitInfo;
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
                
                // Get the hits
                Objects.ForEach((obj) =>
                {
                    hitInfo = obj.Hit(ray);
                    if (hitInfo == null) return;
                    Hits.Add(hitInfo);
                });

                // Draw the pixel
                DrawPixel(colPos, rowPos, Hits, ray.Direction);
            }
        }
        
        Console.WriteLine("Frame rendered");
    }

    public void DrawPixel(int x, int y, List<HitInfo> hits, Vector3 viewDirection)
    {
        if (hits.Count == 0)
        {
            Image[x, y] = new Rgba32(0, 0, 0);
            return;
        }
        
        // Go over each hitInfo and their hits and get the smallest hit time
        var hit = hits.SelectMany(h => h.Hits).OrderBy(h => h.HitTime).First();
        
        // Get the half way vector
        var halfway = Vector3.Normalize(LightDirection + viewDirection);
        
        Image[x, y] = Material.Shade(hit, LightDirection, viewDirection, halfway);
    }
    
    public void AddObject(IRayTraceable obj)
    {
        Objects.Add(obj);
    }
    
    public void RemoveObject(IRayTraceable obj)
    {
        Objects.Remove(obj);
    }
}