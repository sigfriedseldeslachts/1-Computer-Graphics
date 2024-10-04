using System.Numerics;
using RayTracingEngine.Math;

namespace Tests.Math;

public class MatrixTransformationUnitTest
{
    private Random _rnd;
    private Matrix4x4 _testMatrix;
    
    public MatrixTransformationUnitTest()
    {
        _rnd = new Random();
    }
    
    [SetUp]
    public void Setup()
    {
        _testMatrix = new Matrix4x4(1, 2, 3, 4,
                                    5, 6, 7, 8,
                                    9, 10, 11, 12,
                                    13, 14, 15, 16);
    }
    
    [Test]
    public void TestTranslation()
    {
        // Grab 3 random numbers for translation
        var x = _rnd.Next(1, 10);
        var y = _rnd.Next(1, 10);
        var z = _rnd.Next(1, 10);
        
        Console.WriteLine(x);
        Console.WriteLine(y);
        Console.WriteLine(z);
        Console.WriteLine(MatrixTransformation.CreateTranslation(x, y, z));
        
        // Compare my own translation matrix with the System.Numerics one
        Assert.That(
            MatrixTransformation.CreateTranslation(x, y, z),
            Is.EqualTo(Matrix4x4.CreateTranslation(x, y, z))
        );
    }

    [Test]
    public void TestScale()
    {
        var x = _rnd.Next(1, 10);
        var y = _rnd.Next(1, 10);
        var z = _rnd.Next(1, 10);
        
        Assert.That(
            MatrixTransformation.CreateScale(x, y, z),
            Is.EqualTo(Matrix4x4.CreateScale(x, y, z))
        );
    }
}