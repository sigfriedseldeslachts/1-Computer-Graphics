using System.Numerics;

namespace RayTracingEngine.Math;

public class Noise
{
    // Page 652 in the book.

    private int[] IndexTable { get; set; }
    private float[] NoiseTable { get; set; }
    private readonly uint _amount;
    
    public Noise(uint amount = 256)
    {
        _amount = amount;
        GetNoiseTables();
    }

    private void GetNoiseTables()
    {
        // Create an empty array
        IndexTable = new int[_amount];
        
        // Fill the array with values from 0 to _amount
        for (var i = 0; i < _amount; i++)
        {
            IndexTable[i] = i;
        }
        
        // Now shuffle the array
        for (var i = 0; i < _amount; i++)
        {
            var randomIndex = (int) Utils.RandomFloat(0, _amount-1); // -1 to avoid out of bounds
            var temporaryHolder = IndexTable[i];
            IndexTable[randomIndex] = IndexTable[i];
            IndexTable[i] = temporaryHolder;
        }
        
        // Create a random new noise table
        NoiseTable = new float[_amount];
        for (var i = 0; i < _amount; i++)
        {
            NoiseTable[i] = Utils.RandomFloat();
        }
    }

    public float GetLatticeNoise(Vector3 point)
    {
        var index = PermutatePoint((int) point.X + PermutatePoint((int) point.Y + PermutatePoint((int) point.Z)));
        return NoiseTable[index];
    }
    
    public int PermutatePoint(int point)
    {
        return IndexTable[point % _amount];
    }

    /// <summary>
    /// Creates a noise value based on the point and scale.
    /// Page 654 in the book.
    /// </summary>
    /// <param name="scale"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public float GetNoise(float scale, Vector3 point)
    {
        var v = new float[2][][];
        var x = point.X * scale + 10000;
        var y = point.Y * scale + 10000;
        var z = point.Z * scale + 10000;
        
        var ix = (long) x;
        var iy = (long) y;
        var iz = (long) z;
        
        // Get the fractional part of the point
        var fx = x - ix;
        var fy = y - iy;
        var fz = z - iz;
        
        // Get noise at 8 lattice points
        for (var k = 0; k < 2; k++)
        {
            v[k] = new float[2][];
            for (var j = 0; j < 2; j++)
            {
                v[k][j] = new float[2];
                for (var i = 0; i < 2; i++)
                {
                    v[k][j][i] = GetLatticeNoise(new Vector3(ix + i, iy + j, iz + k));
                }
            }
        }
        
        var x0 = Utils.Lerp(v[0][0][0], v[0][0][1], fx);
        var x1 = Utils.Lerp(v[0][1][0], v[0][1][1], fx);
        var x2 = Utils.Lerp(v[1][0][0], v[1][0][1], fx);
        var x3 = Utils.Lerp(v[1][1][0], v[1][1][1], fx);
        var y0 = Utils.Lerp(x0, x1, fy);
        var y1 = Utils.Lerp(x2, x3, fy);
        return Utils.Lerp(y0, y1, fz);
    }
}