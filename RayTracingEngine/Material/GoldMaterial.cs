using System.Numerics;

namespace RayTracingEngine.Material;

public class GoldMaterial : StandardMaterial
{
    // From: learnwebgl.brown37.net/10_surface_properties/surface_properties_color.html
    public GoldMaterial()
    {
        AmbientColor = new Vector4(0.24725f, 0.2245f, 0.0645f, 1.0f);
        DiffuseColor = new(0.34615f, 0.3143f, 0.0903f, 1.0f);
        Fresnel = new(0.797357f, 0.723991f, 0.208006f, 1.0f);
        SurfaceRoughness = 0.832f;
        ReflectionCoefficient = 0.7f;
    }
}