using System.Numerics;

namespace RayTracingEngine.Material;

public class GlassMaterial : StandardMaterial
{
    // From: learnwebgl.brown37.net/10_surface_properties/surface_properties_color.html
    public GlassMaterial()
    {
        AmbientColor = new Vector4(0.1f, 0.1f, 0.1f, 1);
        DiffuseColor = new Vector4(0.1f, 0.1f, 0.1f, 1);
        Fresnel = new Vector4(0.95f, 0.93f, 0.88f, 1);
        ReflectionCoefficient = 0.1f;
        RefractionIndex = 1.5f;
        TransparencyCoefficient = 0.9f;
    }
}