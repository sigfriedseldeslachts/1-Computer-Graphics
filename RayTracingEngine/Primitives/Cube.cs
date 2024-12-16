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
    
    public override float[] SimpleHitLocal(Ray ray)
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
            
            if (MathF.Abs(denominator) == 0)
            {
                if (numenator < 0) return []; // Ray is parallel to the face and outside the cube, no hit
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

        if (tIn >= tOut) return []; // Ray misses the cube
        return [tIn, inSurface, tOut, outSurface];
    }
    
    public override HitPoint[] HitLocal(Ray ray, Ray worldRay)
    {
        var values = SimpleHitLocal(ray);
        if (values.Length == 0) return [];
        Hits.Clear();
        
        if (values[0] > 0.00001)
        {
            Hits.Add(new HitPoint
            {
                Object = this,
                HitTime = values[0],
                Point = worldRay.GetPoint(values[0]),
                Normal = GetCorrectedNormal(GetUnitNormal((int) values[1]), ray.Direction),
                IsEntering = true
            });
        }
        
        if (values[2] > 0.00001 && values[2] < 100000.0f)
        {
            Hits.Add(new HitPoint
            {
                Object = this,
                HitTime = values[2],
                Point = worldRay.GetPoint(values[2]),
                SurfaceIndex = (int) values[3],
                Normal = GetCorrectedNormal(GetUnitNormal((int) values[3]), ray.Direction),
                IsEntering = false
            });
        }
        
        return Hits.ToArray();
    }

    public override bool HasShadowHit(Ray ray)
    {
        var values = SimpleHit(ray);
        
        // Any of the hit points must lie between 0 and 1
        return values.Length != 0 && ((values[0] > 0 && values[0] < 1) || (values[2] > 0 && values[2] < 1));
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