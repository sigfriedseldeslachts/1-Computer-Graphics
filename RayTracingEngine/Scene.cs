using System.Collections.Concurrent;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Scene
{
    public readonly ConcurrentBag<IRayTraceable> Objects = new();
    public readonly ConcurrentBag<Light> Lights = new();

    public bool IsInShadow(Ray ray)
    {
        return Objects.Any(obj => obj.HasShadowHit(ray));
    }

    public HitPoint? GetBestHit(Ray ray)
    {
        HitPoint?[] hitPoints;
        HitPoint? bestHit = null;
        var hitTime = float.MaxValue;
        
        // Go over all objects in the scene
        foreach (var obj in Objects)
        {
            hitPoints = obj.Hit(ray);
            if (hitPoints.Length == 0) continue;
            
            // From the hit points, find the one with the smallest hit time
            foreach (var hit in hitPoints)
            {
                // Skip if the hit time is greater than the current hit time
                if (hit == null || hit.HitTime >= hitTime) continue;
                
                hitTime = hit.HitTime;
                bestHit = hit;
            }
        }

        return bestHit;
    }
    
    public void AddObject(IRayTraceable obj)
    {
        Objects.Add(obj);
    }
}