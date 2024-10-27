using System.Numerics;
using RayTracingEngine.Math;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public abstract class Object : IRayTraceable
{
    public Vector3 GlobalPosition { get; set; }
    public Vector3 Rotation { get; set; }
    public Vector3 Scale { get; set; }
    
    public Matrix4x4 TransformMatrix { get; set; } = Matrix4x4.Identity;
    public Matrix4x4 InverseTransformMatrix { get; set; } = Matrix4x4.Identity;
    
    public abstract HitInfo? HitLocal(Ray ray, bool transformBack = true);

    protected Object(Vector3 position, Vector3 rotation, Vector3 scale)
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
        
        if (Matrix4x4.Invert(TransformMatrix, out var inverseMatrix))
        {
            InverseTransformMatrix = inverseMatrix;
        }
    }
    
    public HitInfo? Hit(Ray ray)
    {
        return HitLocal(ray.Transform(InverseTransformMatrix));
    }
}