using System.Collections;
using System.Numerics;
using Object = RayTracingEngine.Primitives.Object;

namespace RayTracingEngine.Rendering;

public class HitInfo
{
    public Object Object { get; set; }

    public HitPoint[] Hits { get; set; } = Array.Empty<HitPoint>();
}

public class HitPoint
{
    public float HitTime { get; set; }
    public Vector3 Point { get; set; }
    public Vector3 Normal { get; set; }
    public bool IsEntering { get; set; }
    public int SurfaceIndex { get; set; }
}