using System.Numerics;

namespace RayTracingEngine.Material;

public class MetalMaterial : StandardMaterial
{
    // From: learnwebgl.brown37.net/10_surface_properties/surface_properties_color.html
    public MetalMaterial()
    {
        AmbientColor = new Vector4(0.25f, 0.25f, 0.25f, 1);
        DiffuseColor = new Vector4(0.4f, 0.4f, 0.4f, 1);
        Fresnel = new Vector4(0.774597f, 0.774597f, 0.774597f, 1);
        SurfaceRoughness = 0.6f;
        ReflectionCoefficient = 12.8f;
        RefractionIndex = 0;
        TransparencyCoefficient = 0;
    }
}