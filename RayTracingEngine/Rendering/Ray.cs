using System.Numerics;

namespace RayTracingEngine.Rendering;

public class Ray(Vector3 start, Vector3 direction)
{
    public Vector3 Start { get; set; } = start;
    public Vector3 Direction { get; set; } = direction;
    
    public Vector4 StartVec4 => new(Start, 1);
    public Vector4 DirectionVec4 => new(Direction, 0);
    
    public Vector3 GetPoint(float t)
    {
        return Start + t * Direction;
    }
    
    public Ray Transform(Matrix4x4 matrix)
    {
        var transposedMatrix = Matrix4x4.Transpose(matrix); // Transform calculates using the transposed matrix
        
        // Transform Vec3 into Vec4
        var start = Vector4.Transform(StartVec4, transposedMatrix);
        var direction = Vector4.Transform(DirectionVec4, transposedMatrix);
            
        return new Ray(
            new Vector3(start.X, start.Y, start.Z),
            new Vector3(direction.X, direction.Y, direction.Z)
        );
    }
}