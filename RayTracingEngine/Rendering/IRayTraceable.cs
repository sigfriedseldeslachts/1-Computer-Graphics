namespace RayTracingEngine.Rendering;

public interface IRayTraceable
{
    /// <summary>
    /// Check if the ray intersects with the object in local space. Meaning the object is at the origin and is not rotated.
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    HitInfo? HitLocal(Ray ray);
    
    /// <summary>
    /// Check if the ray intersects with the object. The ray is in world space.
    /// The ray should be transformed to local space before calling HitLocal.
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    HitInfo? Hit(Ray ray);
}