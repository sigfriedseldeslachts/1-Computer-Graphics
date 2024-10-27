using System.Numerics;

namespace RayTracingEngine.Rendering;

public class Ray(Vector3 start, Vector3 direction)
{
    public Vector3 Start { get; set; } = start;
    public Vector3 Direction { get; set; } = direction;
    
    public Vector3 GetPoint(float t)
    {
        return Start + t * Direction;
    }
    
    public Ray Transform(Matrix4x4 matrix)
    {
        return new Ray(Vector3.Transform(Start, matrix), Vector3.Transform(Direction, matrix));
    }
}