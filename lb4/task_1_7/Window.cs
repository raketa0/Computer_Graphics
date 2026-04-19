using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Task1
{
    public class Window : GameWindow
    {
        private readonly IcosaDodecahedron shape = new IcosaDodecahedron();

        private bool drag;
        private float lastX, lastY;
        private float rotX, rotY;

        public Window()
            : base(900, 900,
                GraphicsMode.Default,
                "IcosaDodecahedron",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, 1,
                GraphicsContextFlags.Default)
        { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Enable(EnableCap.DepthTest);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-1, 1, -1, 1, 2, 10);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.LoadIdentity();
            GL.Translate(0, 0, -4);

            GL.Rotate(rotX, 1, 0, 0);
            GL.Rotate(rotY, 0, 1, 0);

            shape.Draw();

            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                drag = true;
                lastX = e.X;
                lastY = e.Y;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            drag = false;
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (!drag)
            { 
                return;
            }

            rotY += (e.X - lastX) * 0.25f;
            rotX += (e.Y - lastY) * 0.25f;

            lastX = e.X;
            lastY = e.Y;
        }
    }
}