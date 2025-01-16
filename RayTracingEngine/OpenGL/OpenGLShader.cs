using System.Numerics;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace RayTracingEngine.OpenGL;

public class OpenGLShader
{
    private readonly GL _gl;
    private uint ShaderProgId { get; }

    private const string VertexShader = """
                                         #version 330 core
                                         layout (location = 0) in vec3 aPos;
                                         layout (location = 1) in vec2 aTexCoords;
                                         out vec2 frag_texCoords;

                                         void main()
                                         {
                                             gl_Position = vec4(aPos, 1.0);
                                             
                                             // Passthrough the texture
                                             frag_texCoords = aTexCoords;
                                         }
                                         """;
    private const string FragmentShader = """
                                           #version 330 core
                                           in vec2 frag_texCoords;
                                           out vec4 FragColor;
                                           uniform sampler2D uTexture;

                                           void main()
                                           {
                                               FragColor = texture(uTexture, frag_texCoords);
                                           }
                                           """;

    public OpenGLShader(IWindow window)
    {
        _gl = GL.GetApi(window);
        
        // Load the vertex and fragment shaders
        var vertexShader = CreateShader(VertexShader, ShaderType.VertexShader);
        var fragmentShader = CreateShader(FragmentShader, ShaderType.FragmentShader);
        
        // Compile the shaders
        ShaderProgId = _gl.CreateProgram();
        _gl.AttachShader(ShaderProgId, vertexShader);
        _gl.AttachShader(ShaderProgId, fragmentShader);
        _gl.LinkProgram(ShaderProgId);
        
        // Delete the shader, we don't need it anymore
        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);
        
        CheckForShaderErrors();
    }
    
    public void Dispose()
    {
        _gl.DeleteProgram(ShaderProgId);
    }

    private void CheckForShaderErrors()
    {
        _gl.GetProgram(ShaderProgId, GLEnum.LinkStatus, out var status);
        if (status != 0) return; // No error
        throw new Exception($"Error linking shader program\n{_gl.GetProgramInfoLog(ShaderProgId)}");
    }

    private uint CreateShader(string data, ShaderType type)
    {
        var shader = _gl.CreateShader(type);
        _gl.ShaderSource(shader, data);
        _gl.CompileShader(shader);
        return shader;
    }
    
    public void Use()
    {
        _gl.UseProgram(ShaderProgId);
    }
}