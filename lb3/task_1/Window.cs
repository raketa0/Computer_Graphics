using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace Window
{
    public class Window : GameWindow
    {
        private CoordinateSystem.CoordinateSystem coordinateSystem;
        private BezierCurve.BezierCurve bezierCurve;
        private MouseController.MouseController mouseController;

        public Window()
            : base(800, 600, GraphicsMode.Default, "Bezier")
        {
            VSync = VSyncMode.On;

            coordinateSystem = new CoordinateSystem.CoordinateSystem();

            bezierCurve = new BezierCurve.BezierCurve(new Vector2[]
            {
                new Vector2(-15, -5),
                new Vector2(-5, 8),
                new Vector2(8, -7),
                new Vector2(15, 4)
            });

            mouseController = new MouseController.MouseController(bezierCurve.ControlPoints);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color4.White);
            GL.LineWidth(2f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(-20, 20, -10, 10, -1, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            coordinateSystem.Draw();
            bezierCurve.Draw();

            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            mouseController.MouseDown(e, Width, Height);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            mouseController.MouseUp();
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            mouseController.MouseMove(e, Width, Height);
        }
    }
}