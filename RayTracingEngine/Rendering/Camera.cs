using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.Rendering;

public class Camera(Vector3 position, int width, int height)
{
    public Vector3 Position { get; set; } = position;
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
    
    public Image<Rgba32> Image { get; set; } = new(width, height);
    
    public readonly List<IRayTraceable> Objects = new List<IRayTraceable>();
    public readonly List<HitInfo> Hits = new List<HitInfo>();

    public void Render()
    {
        Image = new Image<Rgba32>(Width, Height);

        int rowPos, colPos;
        HitInfo? hitInfo;
        var ray = new Ray(Position, Vector3.Zero);
        
        for (rowPos = 0; rowPos < Height; rowPos++)
        {
            for (colPos = 0; colPos < Width; colPos++)
            {
                // For each ray we clear the hits
                Hits.Clear();
                
                // We want the center to be X = 0, Y = 0. Thus, we need to shift everything by half the width and height
                var x = colPos - Width / 2;
                var y = rowPos - Height / 2;

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
                DrawPixel(colPos, rowPos, Hits);
            }
        }
        
        Console.WriteLine("Done");
    }

    public void DrawPixel(int x, int y, List<HitInfo> hits)
    {
        if (hits.Count == 0)
        {
            Image[x, y] = new Rgba32(0, 0, 0);
            return;
        }
        
        Image[x, y] = Rgba32.ParseHex("#FF0000");
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