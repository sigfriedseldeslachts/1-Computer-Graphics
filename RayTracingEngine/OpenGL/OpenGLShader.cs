using System.Numerics;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace RayTracingEngine.OpenGL;

public class OpenGLShader
{
    private readonly GL _gl;
    public uint ShaderProgId { get; }
    
    public OpenGLShader(IWindow window, string vertexPath, string fragmentPath)
    {
        _gl = GL.GetApi(window);
        
        // Load the vertex and fragment shaders
        var vertexShader = CreateShader(LoadShader(vertexPath), ShaderType.VertexShader);
        var fragmentShader = CreateShader(LoadShader(fragmentPath), ShaderType.FragmentShader);
        
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
    
    public static string LoadShader(string path)
    {
        return File.ReadAllText(path);
    }
    
    public void Use()
    {
        _gl.UseProgram(ShaderProgId);
    }
    
    public void SetUniformMat3(string name, Matrix4x4 value)
    {
        var location = _gl.GetUniformLocation(ShaderProgId, name);
        if (location == -1) throw new Exception($"{name} uniform not found on shader.");
        unsafe
        {
            _gl.UniformMatrix4(location, 1, false, (float*) &value);
        }
    }
    
    public void SetUniformMat4(string name, Matrix4x4 value)
    {
        var location = _gl.GetUniformLocation(ShaderProgId, name);
        if (location == -1) throw new Exception($"{name} uniform not found on shader.");
        unsafe
        {
            _gl.UniformMatrix4(location, 1, false, (float*) &value);
        }
    }
}