using System.Numerics;

namespace RayTracingEngine.Material;

public class SkyMaterial : StandardMaterial
{
    // From: learnwebgl.brown37.net/10_surface_properties/surface_properties_color.html
    public SkyMaterial()
    {
        AmbientColor = new Vector4(0.1f, 0.18725f, 0.1745f, 0.8f);
        DiffuseColor = new Vector4(0.396f, 0.74151f, 0.69102f, 0.8f);
        Fresnel = new Vector4(0.297254f, 0.30829f, 0.306678f, 0.8f);
    }
}