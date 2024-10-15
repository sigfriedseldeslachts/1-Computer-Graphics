using Object = RayTracingEngine.Primitives.Object;

namespace RayTracingEngine.Rendering;

public class HitInfo
{
    public float HitTime { get; set; }
    public Object Object { get; set; }
    public bool IsEntering { get; set; }
}