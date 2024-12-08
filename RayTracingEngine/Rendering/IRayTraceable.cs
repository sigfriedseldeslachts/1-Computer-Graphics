namespace RayTracingEngine.Rendering;

public interface IRayTraceable
{
    /// <summary>
    /// Tests if there is a hit but does not return the hit points in local space
    /// </summary>
    /// <param name="ray">Ray</param>
    /// <returns></returns>
    float[] SimpleHitLocal(Ray ray);
    
    /// <summary>
    /// Tests if there is a hit and returns the hit points in world space
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    float[] SimpleHit(Ray ray);
    
    /// <summary>
    /// True if there is a hit with the object (ray in world space)
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    bool HasHit(Ray ray);
    
    /// <summary>
    /// Check if the ray intersects with the object in local space. Meaning the object is at the origin and is not rotated.
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="transformBack">Set to true if the hit point should be transformed back to world space</param>
    /// <returns></returns>
    HitPoint[] HitLocal(Ray ray, bool transformBack = true);
    
    /// <summary>
    /// Check if the ray intersects with the object. The ray is in world space.
    /// The ray should be transformed to local space before calling HitLocal.
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    HitPoint[] Hit(Ray ray);
}