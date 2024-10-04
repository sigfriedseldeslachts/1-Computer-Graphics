using System.Numerics;

namespace RayTracingEngine.Math;

public class MatrixTransformation
{
    /// <summary>
    /// For translation of a matrix
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Matrix4x4 CreateTranslation(float x, float y, float z)
    {
        // 1 0 0 x
        // 0 1 0 y
        // 0 0 1 z
        // 0 0 0 1
        return new Matrix4x4(1, 0, 0, x,
                             0, 1, 0, y,
                             0, 0, 1, z,
                             0, 0, 0, 1);
    }

    /// <summary>
    /// For scaling a matrix
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Matrix4x4 CreateScale(float x = 1.0f, float y = 1.0f, float z = 1.0f)
    {
        // x 0 0 0
        // 0 y 0 0
        // 0 0 z 0
        // 0 0 0 1
        return new Matrix4x4(x, 0, 0, 0,
                             0, y, 0, 0,
                             0, 0, z, 0,
                             0, 0, 0, 1);
    }
    
    /// <summary>
    /// For a rotation around the Z axis
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Matrix4x4 CreateRotationZ(float angle)
    {
        // cos -sin 0 0
        // sin  cos 0 0
        // 0    0   1 0
        // 0    0   0 1
        var cos = (float)System.Math.Cos(angle);
        var sin = (float)System.Math.Sin(angle);
        
        return new Matrix4x4(cos, -sin, 0, 0,
                             sin, cos,  0, 0,
                             0,    0,   1, 0,
                             0,    0,   0, 1);
    }
    
    /// <summary>
    /// For a rotation around the Y axis
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Matrix4x4 CreateRotationY(float angle)
    {
        // cos 0 sin 0
        // 0   1 0   0
        // -sin 0 cos 0
        // 0   0 0   1
        var cos = (float)System.Math.Cos(angle);
        var sin = (float)System.Math.Sin(angle);
        
        return new Matrix4x4(cos, 0, sin, 0,
                             0,   1, 0,   0,
                             -sin, 0, cos, 0,
                             0,   0, 0,   1);
    }
    
    /// <summary>
    /// For a rotation around the X axis
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Matrix4x4 CreateRotationX(float angle)
    {
        // 1 0    0   0
        // 0 cos -sin 0
        // 0 sin  cos 0
        // 0 0    0   1
        var cos = (float)System.Math.Cos(angle);
        var sin = (float)System.Math.Sin(angle);
        
        return new Matrix4x4(1, 0,    0,   0,
                             0, cos, -sin, 0,
                             0, sin,  cos, 0,
                             0, 0,    0,   1);
    }
    
    
}