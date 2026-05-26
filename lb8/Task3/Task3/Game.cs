using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RayTracer;

public class Game : GameWindow
{
    private Renderer renderer;

    public float cameraAngleX = 0f;
    public float cameraAngleY = 0.3f;

    private bool dragging;
    private Vector2 lastMouse;

    public Game(int w, int h)
        : base(GameWindowSettings.Default,
            new NativeWindowSettings
            {
                Size = new Vector2i(w, h),
                Title = "Torus Pyramid"
            })
    { }

    protected override void OnLoad()
    {
        renderer = new Renderer(Size.X, Size.Y, this);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        var mouse = MouseState;

        if (mouse.IsButtonDown(MouseButton.Left))
        {
            if (!dragging)
            {
                dragging = true;
                lastMouse = new Vector2(mouse.X, mouse.Y);
            }
            else
            {
                var d = new Vector2(mouse.X, mouse.Y) - lastMouse;

                cameraAngleX += d.X * 0.01f;
                cameraAngleY -= d.Y * 0.01f;

                cameraAngleY = MathHelper.Clamp(cameraAngleY, -1.2f, 1.2f);

                lastMouse = new Vector2(mouse.X, mouse.Y);
            }
        }
        else dragging = false;
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        renderer.Render();
        SwapBuffers();
    }
}