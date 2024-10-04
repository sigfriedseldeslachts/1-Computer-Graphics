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
        
        // Matrix4x4 transposes the matrix (this allows for easier multiplication)
        var libraryMatrix = Matrix4x4.Transpose(Matrix4x4.CreateTranslation(x, y, z));
        
        // Compare my own translation matrix with the System.Numerics one
        Assert.That(
            MatrixTransformation.CreateTranslation(x, y, z),
            Is.EqualTo(libraryMatrix)
        );
    }

    [Test]
    public void TestScale()
    {
        var x = _rnd.Next(1, 10);
        var y = _rnd.Next(1, 10);
        var z = _rnd.Next(1, 10);
        
        // Undo transposition
        var libraryMatrix = Matrix4x4.Transpose(Matrix4x4.CreateScale(x, y, z));
        
        Assert.That(
            MatrixTransformation.CreateScale(x, y, z),
            Is.EqualTo(libraryMatrix)
        );
    }
    
    [Test]
    public void TestRotation()
    {
        // Create random angle in radians
        var x = (float)_rnd.NextDouble();
        var y = (float)_rnd.NextDouble();
        var z = (float)_rnd.NextDouble();

        // Test X
        var myMatrixX = MatrixTransformation.CreateRotationX(x);
        var libraryMatrixX = Matrix4x4.Transpose(Matrix4x4.CreateRotationX(x));
        Assert.That(
            myMatrixX,
            Is.EqualTo(libraryMatrixX)
        );
        
        // Test Y
        var myMatrixY = MatrixTransformation.CreateRotationY(x);
        var libraryMatrixY = Matrix4x4.Transpose(Matrix4x4.CreateRotationY(x));
        Assert.That(
            myMatrixY,
            Is.EqualTo(libraryMatrixY)
        );
        
        // Test Z
        var myMatrixZ = MatrixTransformation.CreateRotationZ(x);
        var libraryMatrixZ = Matrix4x4.Transpose(Matrix4x4.CreateRotationZ(x));
        Assert.That(
            myMatrixZ,
            Is.EqualTo(libraryMatrixZ)
        );
        
        // Test all 3 rotations
        var libraryMatrixMultiplied = Matrix4x4.Transpose(
            Matrix4x4.CreateRotationZ(z) *
            Matrix4x4.CreateRotationY(y) *
            Matrix4x4.CreateRotationX(x)
        );
        Assert.That(
            MatrixTransformation.CreateRotation(x, y, z),
            Is.EqualTo(libraryMatrixMultiplied).Within(0.0001),
            "Multiplication of X, Y, and Z rotation matrices should be correct"
        );
    }
}