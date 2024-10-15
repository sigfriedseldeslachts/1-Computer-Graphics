using System.Numerics;

namespace RayTracingEngine.Rendering;

public class Ray(Vector3 start, Vector3 direction)
{
    public Vector3 Start { get; set; } = start;
    public Vector3 Direction { get; set; } = direction;
}