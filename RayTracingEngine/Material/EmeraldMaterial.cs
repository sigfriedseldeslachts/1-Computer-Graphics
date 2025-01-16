using System.Numerics;

namespace RayTracingEngine.Material;

public class EmeraldMaterial : StandardMaterial
{
    // From: learnwebgl.brown37.net/10_surface_properties/surface_properties_color.html
    public EmeraldMaterial()
    {
        AmbientColor = new Vector4(0.0215f, 0.1745f, 0.0215f, 0.55f);
        DiffuseColor = new Vector4(0.07568f, 0.61424f, 0.07568f, 0.55f);
        Fresnel = new Vector4(0.633f, 0.727811f, 0.633f, 0.55f);
        SurfaceRoughness = 0.832f;
        ReflectionCoefficient = 0.6f;
        RefractionIndex = 1.602f;
        TransparencyCoefficient = 0.7f;
    }
}