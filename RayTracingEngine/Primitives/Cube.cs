using System.Numerics;
using RayTracingEngine.Math;
using RayTracingEngine.Rendering;

namespace RayTracingEngine.Primitives;

public class Cube : AObject
{
    public Cube(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
    {
        BuildTransformMatrix(position, rotation, scale);
    }
    
    public override HitInfo? HitLocal(Ray ray, bool transformBack = true)
    {
        int inSurface = 0, outSurface = 0;
        float tIn = float.MinValue, tOut = float.MaxValue, numenator = 0, denominator = 0;
        
        for (var i = 0; i < 6; i++)
        {
            // Decide which face to check
            switch (i)
            {
                case 0:
                    numenator = 1.0f - ray.Start.Y;
                    denominator = ray.Direction.Y;
                    break;
                case 1:
                    numenator = 1.0f + ray.Start.Y;
                    denominator = -ray.Direction.Y;
                    break;
                case 2:
                    numenator = 1.0f - ray.Start.X;
                    denominator = ray.Direction.X;
                    break;
                case 3:
                    numenator = 1.0f + ray.Start.X;
                    denominator = -ray.Direction.X;
                    break;
                case 4:
                    numenator = 1.0f - ray.Start.Z;
                    denominator = ray.Direction.Z;
                    break;
                case 5:
                    numenator = 1.0f + ray.Start.Z;
                    denominator = -ray.Direction.Z;
                    break;
            }
            
            if (MathF.Abs(denominator) < 0.00001)
            {
                // Ray is parallel to the face
                continue;
            }
            
            var t = numenator / denominator;
            if (denominator > 0) // This happens when our ray is exiting the cube
            {
                if (t >= tOut) continue;
                tOut = t;
                outSurface = i;
            }
            else // This happens when our ray is entering the cube
            {
                if (t <= tIn) continue;
                tIn = t;
                inSurface = i;
            }
        }

        if (tIn >= tOut || tIn == float.MinValue || tOut == float.MaxValue) return null;
        var hits = new List<HitPoint>();
        
        if (tIn > 0.00001)
        {
            hits.Add(new HitPoint
            {
                HitTime = tIn,
                Point = GetHitPoint(ray.GetPoint(tIn), transformBack),
                Normal = GetUnitNormal(inSurface),
                IsEntering = true
            });
        }
        
        if (tOut > 0.00001)
        {
            hits.Add(new HitPoint
            {
                HitTime = tOut,
                Point = GetHitPoint(ray.GetPoint(tOut), transformBack),
                SurfaceIndex = outSurface,
                Normal = GetUnitNormal(outSurface),
                IsEntering = false
            });
        }
        
        if (hits.Count == 0) return null;
        return new HitInfo
        {
            AObject = this,
            Hits = hits.ToArray()
        };
    }

    public override Vector3 GetHitPoint(Vector3 point, bool transformBack)
    {
        return transformBack ? Vector3.Transform(point, TransposedInverseTransformMatrix) : point;
    }

    public static Vector3 GetUnitNormal(int surfaceIndex)
    {
        return surfaceIndex switch
        {
            0 => new Vector3(0, 1, 0),
            1 => new Vector3(0, -1, 0),
            2 => new Vector3(1, 0, 0),
            3 => new Vector3(-1, 0, 0),
            4 => new Vector3(0, 0, 1),
            5 => new Vector3(0, 0, -1),
            _ => Vector3.Zero
        };
    }
}