using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace Tests.Primitives;

public class SphereIntersectionUnitTest
{
    [Test]
    public void TestNoIntersectionInLocalSpace()
    {
        var sphere = new Sphere(Vector3.Zero, Vector3.Zero, Vector3.One);
        var ray = new Ray(new Vector3(1, 0.75f, 0.75f), Vector3.UnitX);
        
        Assert.That(
            sphere.HitLocal(ray, false),
            Is.Null
        );
    }

    [Test]
    public void TestIntersectionTwoPointsInLocalSpace()
    {
        var sphere = new Sphere(Vector3.Zero, Vector3.Zero, Vector3.One);
        var ray = new Ray(Vector3.One, -1 * Vector3.One);

        var hit = sphere.HitLocal(ray, false);
        Assert.That(hit, Is.Not.Null);
        Assert.That(hit.Length, Is.EqualTo(2));
    }

    [Test]
    public void TestIntersectionTouchingInLocalSpace()
    {
        var sphere = new Sphere(Vector3.Zero, Vector3.Zero, Vector3.One);
        var ray = new Ray(Vector3.UnitX, Vector3.UnitY);
        
        var hit = sphere.HitLocal(ray, false);
        Assert.That(hit, Is.Not.Null);
        Assert.That(hit!.Length, Is.EqualTo(1));
    }
    
    [Test]
    public void TestRayStartingFromInsideCube()
    {
        var sphere = new Sphere(Vector3.Zero, Vector3.Zero, new Vector3(5, 5, 5));
        var ray = new Ray(Vector3.Zero, Vector3.UnitX);
        
        var hit = sphere.HitLocal(ray, false);
        Assert.That(hit, Is.Not.Null);
        Assert.That(hit!.Length, Is.EqualTo(1));
    }
}