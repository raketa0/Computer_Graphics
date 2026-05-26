using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Task2;

public class Window : GameWindow
{
    int vao;
    int vbo;

    ShaderScreen shader;

    float theta = 0.0f;
    float phi = 0.3f;

    Vector2 lastMouse;

    bool firstMove = true;

    const float distance = 10.0f;

    public Window(
        GameWindowSettings g,
        NativeWindowSettings n
    ) : base(g, n)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0f, 0f, 0f, 1f);

        shader = new ShaderScreen();

        float[] quad =
        {
            -1f,-1f,
             1f,-1f,
             1f, 1f,

            -1f,-1f,
             1f, 1f,
            -1f, 1f
        };

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        GL.BufferData(
            BufferTarget.ArrayBuffer,
            quad.Length * sizeof(float),
            quad,
            BufferUsageHint.StaticDraw
        );

        GL.VertexAttribPointer(
            0,
            2,
            VertexAttribPointerType.Float,
            false,
            2 * sizeof(float),
            0
        );

        GL.EnableVertexAttribArray(0);
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

    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();

        shader.SetFloat("theta", theta);
        shader.SetFloat("phi", phi);
        shader.SetFloat("distance", distance);

        shader.SetVector2(
            "resolution",
            Size.X,
            Size.Y
        );

        GL.BindVertexArray(vao);

        GL.DrawArrays(
            PrimitiveType.Triangles,
            0,
            6
        );

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteBuffer(vbo);
        GL.DeleteVertexArray(vao);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
    }
}