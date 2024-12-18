using System.Numerics;
using RayTracingEngine.Material;
using RayTracingEngine.Math;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public abstract class AObject : IRayTraceable
{
    public Vector3 GlobalPosition { get; set; }
    public Vector3 Rotation { get; set; }
    public Vector3 Scale { get; set; }
    
    public readonly List<HitPoint> Hits = [];
    
    public Matrix4x4 TransformMatrix { get; set; } = Matrix4x4.Identity;
    public Matrix4x4 InverseTransformMatrix { get; set; } = Matrix4x4.Identity;
    
    public StandardMaterial Material { get; set; } = new();
    
    public abstract float[] SimpleHitLocal(Ray ray);
    
    public abstract HitPoint[] HitLocal(Ray ray, Ray worldRay);

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

        InverseTransformMatrix = inverseMatrix;  //Matrix4x4.Transpose(inverseMatrix);
    }

    public abstract bool HasShadowHit(Ray ray);
    
    public float[] SimpleHit(Ray ray)
    {
        return SimpleHitLocal(ray.Transform(InverseTransformMatrix));
    }
    
    public HitPoint[] Hit(Ray ray)
    {
        return HitLocal(ray.Transform(InverseTransformMatrix), ray);
    }
    
    /// <summary>
    /// Inverses the normal if we are hitting the object from the inside
    /// A simple dot product is used to determine this. The normal MUST be in the opposite direction of the ray, if
    /// it is not then we are hitting the object from the inside
    /// </summary>
    /// <param name="normal"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    protected static Vector3 GetCorrectedNormal(Vector3 normal, Vector3 direction)
    {
        return Vector3.Dot(normal, direction) < 0 ? normal : -normal;
    }
}