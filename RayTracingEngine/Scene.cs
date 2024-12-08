using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Scene
{
    public readonly List<IRayTraceable> Objects = [];
    public readonly List<Light> Lights = [];

    public bool IsInShadow(Ray ray)
    {
        foreach (var obj in Objects)
        {
            if (obj.HasHit(ray))
            {
                return true;
            }
        }

        return false;
    }
    
    public void AddObject(IRayTraceable obj)
    {
        Objects.Add(obj);
    }
    
    public void RemoveObject(IRayTraceable obj)
    {
        Objects.Remove(obj);
    }
}