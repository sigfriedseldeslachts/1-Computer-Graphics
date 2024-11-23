using System.Collections;
using System.Numerics;
using RayTracingEngine.Primitives;

namespace RayTracingEngine.Rendering;

public class HitPoint
{
    public AObject Object { get; set; }
    public float HitTime { get; set; }
    public Vector3 Point { get; set; }
    public Vector3 Normal { get; set; }
    public bool IsEntering { get; set; }
    public int SurfaceIndex { get; set; }
}