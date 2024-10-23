using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace RayTracingEngine.OpenGL;

public class OpenGLRenderer
{
    private static float[] vertices = {
        1,  1, 0,  // top right
        1, -1, 0,  // bottom right
       -1, -1, 0,  // bottom left
       -1,  1, 0   // top left 
    };
    private static int[] indices = [0,1,2, 0,2,3];
    
    private readonly GL _gl;
    private readonly OpenGLShader _shader;
    private uint _vao, _vbo, _ebo;

    public OpenGLRenderer(IWindow window)
    {
        _gl = GL.GetApi(window);
        _shader = new OpenGLShader(window,
            "/home/sigfried/Projects/School/RayTracingEngine/RayTracingEngine/OpenGL/vertex.glsl",
            "/home/sigfried/Projects/School/RayTracingEngine/RayTracingEngine/OpenGL/fragment.glsl");
        
        Create2DPlane();
    }
    
    public void Dispose()
    {
        _gl.DeleteBuffer(_vao);
        _gl.DeleteBuffer(_vbo);
        _gl.DeleteBuffer(_ebo);
        _shader.Dispose();
    }

    private unsafe void Create2DPlane()
    {
        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);
        
        // Vertex Buffer Object
        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        fixed (void* v = vertices)
        {
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) vertices.Length * 32, v, BufferUsageARB.StaticDraw);
        }
        
        // Element Array Object
        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
        fixed (void* i = indices)
        {
            _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) indices.Length * 4, i, BufferUsageARB.StaticDraw);
        }
        _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
        _gl.EnableVertexAttribArray(0);
    }
    
    public unsafe void Draw()
    {
        _gl.Clear((uint) ClearBufferMask.ColorBufferBit);
        _gl.ClearColor(0, 0, 0, 1.0f);
        
        _gl.BindVertexArray(_vao);
        _shader.Use();
        
        _gl.DrawElements(GLEnum.Triangles, (uint) indices.Length, GLEnum.UnsignedInt, null);
    }
}