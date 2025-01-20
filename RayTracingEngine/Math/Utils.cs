using System.Numerics;

namespace RayTracingEngine.Math;

public class Utils
{
    public static float[] ClampRgbaValues(float[] values, float min = 0.0f, float max = 1.0f)
    {
        return
        [
            /*System.Math.Clamp(values[0], min, max),
            System.Math.Clamp(values[1], min, max),
            System.Math.Clamp(values[2], min, max),
            System.Math.Clamp(values[3], min, max)*/
            
            float.IsNaN(values[0]) ? 0 : System.Math.Clamp(values[0], min, max),
               float.IsNaN(values[1]) ? 0 : System.Math.Clamp(values[1], min, max),
               float.IsNaN(values[2]) ? 0 : System.Math.Clamp(values[2], min, max),
               float.IsNaN(values[3]) ? 0 : System.Math.Clamp(values[3], min, max)
        ];
    }
    
    public static float RandomFloat(float min = 0.0f, float max = 1.0f)
    {
        return (float) new Random().NextDouble() * (max - min) + min;
    }
    
    public static float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }
}