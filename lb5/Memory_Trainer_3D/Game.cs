using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Numerics;
using System.Reflection;

public class Game : GameWindow
{
    Board board;
    GameLogic logic;
    Renderer renderer;
    Camera camera;
    MousePicker picker;

    bool mousePressed;

    public Game(int w, int h)
        : base(w, h, GraphicsMode.Default, "Memory 3D")
    {
    }

    protected override void OnLoad(System.EventArgs e)
    {
        GL.ClearColor(0.1f, 0.1f, 0.15f, 1);
        GL.Enable(EnableCap.DepthTest);

  

        board = new Board(3, 4);
        logic = new GameLogic();
        renderer = new Renderer();
        camera = new Camera();
        picker = new MousePicker();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        float dt = (float)e.Time;

        var mouse = Mouse.GetState();

        if (mouse.IsButtonDown(MouseButton.Left) && !mousePressed && !board.IsAnimating())
        {
            mousePressed = true;

            var m = Mouse.GetCursorState();
            var p = PointToClient(new Point(m.X, m.Y));

            var ray = picker.GetRay(new OpenTK.Vector2(p.X, p.Y), camera, Width, Height);
            var hit = picker.IntersectPlane(ray);
            var tile = picker.Pick(board, hit);

            if (tile != null)
            {
                logic.OnClick(tile);
            }
        }

        if (mouse.IsButtonUp(MouseButton.Left))
        {
            mousePressed = false;
        }

        board.Update(dt);
        logic.Update(dt);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var view = camera.GetView();
        var proj = camera.GetProjection(Width, Height);

        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref proj);

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref view);

        renderer.Draw(board);

        SwapBuffers();
    }
}