using System.Numerics;
using RayTracingEngine.Math;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public abstract class Object : IRayTraceable
{
    public Vector3 GlobalPosition { get; set; } = Vector3.Zero;
    
    public Matrix4x4 TransformMatrix { get; set; } = Matrix4x4.Identity;
    public Matrix4x4 InverseTransformMatrix { get; set; } = Matrix4x4.Identity;
    
    public abstract HitInfo? HitLocal(Ray ray);

    protected Object(Vector3 position)
    {
        GlobalPosition = position;
    }
    
    protected void BuildTransformMatrix(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        TransformMatrix = MatrixTransformation.CreateTranslation(position);
        
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