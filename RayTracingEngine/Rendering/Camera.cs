using System.Numerics;

namespace RayTracingEngine.Rendering;

public class Camera
{
    private readonly uint _width = 640;
    private readonly uint _height = 480;

    public Vector3 Position { get; set; } = Vector3.Zero;

    public void Render()
    {
        for (uint y = 0; y < _height; y++)
        {
            for (uint x = 0; x < _width; x++)
            {
                // Calculate the ray direction
                var rayDirection = new Vector3(
                    x / (float)_width - 0.5f,
                    y / (float)_height - 0.5f,
                    1 // Distance to the screen
                );

                // Create the ray
                var ray = new Ray(Position, rayDirection);
            }
        }
    }
}