using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace Tests.Primitives;

public class CubeIntersectionUnitTest
{
    [Test]
    public void TestNoIntersectionInLocalSpace()
    {
        var cube = new Cube(Vector3.Zero, Vector3.Zero, Vector3.One);
        var ray = new Ray(new Vector3(2,2,2),  Vector3.UnitX);
        
        Assert.That(
            cube.HitLocal(ray, ray),
            Is.Empty
        );
    }

    [Test]
    public void TestIntersectionTwoPointsInLocalSpace()
    {
        var cube = new Cube(Vector3.Zero, Vector3.Zero, Vector3.One);
        var ray = new Ray(2*Vector3.One, -Vector3.One);

        var hit = cube.HitLocal(ray, ray);
        Assert.That(hit, Is.Not.Null);
        Assert.That(hit, Has.Length.EqualTo(2));
    }

    [Test]
    public void TestIntersectionTouchingInLocalSpace()
    {
        var cube = new Cube(Vector3.Zero, Vector3.Zero, Vector3.One);
        var ray = new Ray(2 * Vector3.UnitX, Vector3.UnitY);
        
        var hit = cube.HitLocal(ray, ray);
        Assert.That(hit, Is.Not.Null);
        Assert.That(hit, Has.Length.EqualTo(1));
    }
    
    [Test]
    public void TestRayStartingFromInsideCube()
    {
        var sphere = new Cube(Vector3.Zero, Vector3.Zero, new Vector3(5, 5, 5));
        var ray = new Ray(Vector3.Zero, Vector3.UnitX);
        
        var hit = sphere.HitLocal(ray, ray);
        Assert.That(hit, Is.Not.Null);
        Assert.That(hit, Has.Length.EqualTo(1));
    }
}