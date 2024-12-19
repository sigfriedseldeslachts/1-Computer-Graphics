using System.Collections.Concurrent;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Scene
{
    public readonly ConcurrentBag<IRayTraceable> Objects = new();
    public readonly ConcurrentBag<Light> Lights = new();
    
    private HitPoint[] _hitPoints = null!;
    private float _hitTime = float.MaxValue;
    private HitPoint? _bestHit = null;

    public bool IsInShadow(Ray ray)
    {
        foreach (var obj in Objects)
        {
            if (obj.HasShadowHit(ray))
            {
                return true;
            }
        }

        return false;
    }

    public HitPoint? GetBestHit(Ray ray)
    {
        _bestHit = null;
        _hitTime = float.MaxValue;
        
        // Go over all objects in the scene
        foreach (var obj in Objects)
        {
            _hitPoints = obj.Hit(ray);
            if (_hitPoints.Length == 0) continue;
            
            // From the hit points, find the one with the smallest hit time
            foreach (var hit in _hitPoints)
            {
                if (hit.HitTime < _hitTime)
                {
                    _hitTime = hit.HitTime;
                    _bestHit = hit;
                }
            }
        }

        return _bestHit;
    }
    
    public void AddObject(IRayTraceable obj)
    {
        Objects.Add(obj);
    }
    
    public void RemoveObject(IRayTraceable obj)
    {
        
    }
}