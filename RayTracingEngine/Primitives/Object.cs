using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public abstract class Object : IRayTraceable
{
    public abstract HitInfo? HitLocal(Ray ray);

    public HitInfo? Hit(Ray ray)
    {
        return HitLocal(ray);
    }
}