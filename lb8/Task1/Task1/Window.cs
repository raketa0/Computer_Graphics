using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Task1;

public class Window : GameWindow
{
    RayTracer rt;
    int tex, vao, vbo;
    ShaderScreen shader;
    byte[] cache;
    bool dirty = true;

    const float distance = 5.0f;
    float theta = 0.0f;
    float phi = 0.3f;
    Vector2 lastMouse;
    bool firstMove = true;

    public Window(GameWindowSettings g, NativeWindowSettings n) : base(g, n)
    {
        rt = new RayTracer();

        Material sphereMat = new Material(
         new Vector3(0.1f, 0.1f, 0.1f),   // ambient
         new Vector3(0.0f, 0.5f, 1.0f),   // diffuse (голубой)
         new Vector3(1.0f, 1.0f, 1.0f),   // specular (белый блик)
         64.0f                              // shininess
     );

        rt.Scene.Add(new Sphere(new Vector3(0, 0, 0), 1.2f, sphereMat));
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0, 0, 0, 1);
        CursorState = CursorState.Normal;

        shader = new ShaderScreen();

        float[] quad =
        {
            -1,-1, 0,0,
             1,-1, 1,0,
             1, 1, 1,1,

            -1,-1, 0,0,
             1, 1, 1,1,
            -1, 1, 0,1
        };

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();
        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, quad.Length * sizeof(float), quad, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        tex = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, tex);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        UpdateCamera();
    }

    void UpdateCamera()
    {
        Vector3 target = new Vector3(0, -0.3f, 0);

        float x = distance * MathF.Cos(phi) * MathF.Sin(theta);
        float y = distance * MathF.Sin(phi);
        float z = distance * MathF.Cos(phi) * MathF.Cos(theta);

        rt.Cam = target + new Vector3(x, y, z);

        Vector3 forward = Vector3.Normalize(target - rt.Cam);
        Vector3 right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
        Vector3 up = Vector3.Cross(right, forward);

        rt.Forward = forward;
        rt.Right = right;
        rt.Up = up;

        dirty = true;
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        base.OnMouseMove(e);
        if (firstMove)
        {
            lastMouse = e.Position;
            firstMove = false;
            return;
        }

        Vector2 delta = e.Position - lastMouse;
        lastMouse = e.Position;

        const float sensitivity = 0.005f;
        theta -= delta.X * sensitivity;

        UpdateCamera();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        if (dirty)
        {
            cache = rt.Render();
            dirty = false;
        }

        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.Viewport(0, 0, Size.X, Size.Y);
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, tex);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, rt.W, rt.H, 0, PixelFormat.Rgb, PixelType.UnsignedByte, cache);
        shader.Use();
        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        GL.DeleteTexture(tex);
        GL.DeleteBuffer(vbo);
        GL.DeleteVertexArray(vao);
    }
}