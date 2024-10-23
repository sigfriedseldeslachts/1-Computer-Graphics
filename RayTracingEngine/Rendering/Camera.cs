using System.Numerics;

namespace RayTracingEngine.Rendering;

public class Camera(Vector3 position, int width, int height)
{
    public Vector3 Position { get; set; } = position;
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
    
    public readonly List<IRayTraceable> Objects = new List<IRayTraceable>();
    public readonly List<HitInfo> Hits = new List<HitInfo>();

    public void Render()
    {
        Hits.Clear();
        
        for (var rowPos = 0; rowPos < Height; rowPos++)
        {
            for (var colPos = 0; colPos < Width; colPos++)
            {
                // We want the center to be X = 0, Y = 0. Thus, we need to shift everything by half the width and height
                var x = colPos - Width / 2;
                var y = rowPos - Height / 2;

                // Now we need to create a ray direction
                var direction = Vector3.Normalize(new Vector3(x, y, -position.Z));
                var ray = new Ray(Position, direction);

                // For each object, check if the ray intersects with it
                foreach (var hitInfo in Objects.Select(obj => obj.Hit(ray)).OfType<HitInfo>())
                {
                    Hits.Add(hitInfo);
                    Console.WriteLine("Hit at ray originating from x: {0}, y: {1}", x, y);
                }
            }
        }
    }
    
    public void AddObject(IRayTraceable obj)
    {
        Objects.Add(obj);
    }
}