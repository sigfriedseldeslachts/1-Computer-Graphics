using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace Tests.Primitives;

public class CubeIntersectionUnitTest
{
    [Test]
    public void TestNoIntersectionInLocalSpace()
    {
        var cube = new Cube(Vector3.Zero, 1);
        var ray = new Ray(new Vector3(2,2,2),  Vector3.UnitX);
        
        Assert.That(
            cube.HitLocal(ray),
            Is.Null
        );
    }

    [Test]
    public void TestIntersectionTwoPointsInLocalSpace()
    {
        var cube = new Sphere(Vector3.Zero, 1);
        var ray = new Ray(Vector3.One, -1 * Vector3.One);

        var hit = cube.HitLocal(ray);
        Assert.That(hit, Is.Not.Null);
        Assert.That(hit!.Hits.Count, Is.EqualTo(2));
    }

    [Test]
    public void TestIntersectionTouchingInLocalSpace()
    {
        var cube = new Sphere(Vector3.Zero, 1);
        var ray = new Ray(Vector3.UnitX, Vector3.UnitY);
        
        var hit = cube.HitLocal(ray);
        Assert.That(hit, Is.Not.Null);
        Assert.That(hit!.Hits.Count, Is.EqualTo(1));
    }
}