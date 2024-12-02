using System.Runtime.CompilerServices;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingEngine.OpenGL;

public class OpenGlRenderer
{
    private static float[] vertices = {
        // Positions  // Texture Coords
        1,  1, 0,     1, 1, // top right
        1, -1, 0,     1, 0, // bottom right
       -1, -1, 0,     0, 0, // bottom left
       -1,  1, 0,     0, 1  // top left 
    };
    private static int[] indices = [0,1,2, 0,2,3];
    
    private readonly GL _gl;
    private readonly OpenGLShader _shader;
    private uint _vao, _vbo, _ebo, _texture;
    private byte[] _pixelBuffer = [];

    public OpenGlRenderer(IWindow window)
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
        
        //// Vertex Attribute Pointers
        // Position attribute
        _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), null);
        _gl.EnableVertexAttribArray(0);
        // Texture coord attribute
        _gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*) (3 * sizeof(float)));
        _gl.EnableVertexAttribArray(1);
        
        // Create texture object
        _texture = _gl.GenTexture();
        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, _texture);
    }
    
    public unsafe void SetTextureFromBitmap(Image<Rgba32> image)
    {
        // Get the pixel buffer
        _pixelBuffer = new byte[image.Width * image.Height * Unsafe.SizeOf<Rgba32>()];
        image.CopyPixelDataTo(_pixelBuffer);
        
        _gl.BindTexture(TextureTarget.Texture2D, _texture);
        fixed (byte* ptr = _pixelBuffer)
        {
            _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba32f, (uint) image.Width, (uint) image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
        }
        _gl.GenerateMipmap(TextureTarget.Texture2D);
        
        // Clear the pixel buffer
        _pixelBuffer = [];
    }
    
    public unsafe void Draw()
    {
        _gl.Clear((uint) ClearBufferMask.ColorBufferBit);
        _gl.ClearColor(0, 0, 0, 1.0f);
        
        _gl.BindVertexArray(_vao);
        _shader.Use();
        
        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, _texture);
        
        _gl.DrawElements(GLEnum.Triangles, (uint) indices.Length, GLEnum.UnsignedInt, null);
    }
}