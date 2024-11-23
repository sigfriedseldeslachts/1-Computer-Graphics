using System.Numerics;
using RayTracingEngine.Math;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public abstract class AObject : IRayTraceable
{
    public Vector3 GlobalPosition { get; set; }
    public Vector3 Rotation { get; set; }
    public Vector3 Scale { get; set; }
    
    public Matrix4x4 TransformMatrix { get; set; } = Matrix4x4.Identity;
    public Matrix4x4 InverseTransformMatrix { get; set; } = Matrix4x4.Identity;
    public Matrix4x4 TransposedInverseTransformMatrix { get; set; } = Matrix4x4.Identity;
    
    public abstract HitPoint[] HitLocal(Ray ray, bool transformBack = true);

    public abstract Vector3 GetHitPoint(Vector3 point, bool transformBack);

    protected AObject(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        GlobalPosition = position;
        Rotation = rotation;
        Scale = scale;
    }
    
    protected void BuildTransformMatrix(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        TransformMatrix = MatrixTransformation.CreateTranslation(position)
            * MatrixTransformation.CreateRotation(rotation)
            * MatrixTransformation.CreateScale(scale);

        if (!Matrix4x4.Invert(TransformMatrix, out var inverseMatrix)) return;
        
        InverseTransformMatrix = inverseMatrix;
        TransposedInverseTransformMatrix = Matrix4x4.Transpose(inverseMatrix);
    }
    
    public HitPoint[] Hit(Ray ray)
    {
        return HitLocal(ray.Transform(InverseTransformMatrix));
    }
    
    /// <summary>
    /// Inverses the normal if we are hitting the object from the inside
    /// A simple dot product is used to determine this. The normal MUST be in the opposite direction of the ray, if
    /// it is not then we are hitting the object from the inside
    /// </summary>
    /// <param name="normal"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetCorrectedNormal(Vector3 normal, Vector3 direction)
    {
        return Vector3.Dot(normal, direction) < 0 ? normal : -normal;
    }
}