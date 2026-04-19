using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Task2
{
    class Window : GameWindow
    {
        private MobiusStrip mobiusStrip;

        public Window() : base(1024, 768, GraphicsMode.Default, "Mobius")
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.1f, 0.1f, 0.15f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            mobiusStrip = new MobiusStrip();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            float aspect = Width / (float)Height;

            // Перспективная проекция для 3D
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                aspect,
                0.1f,
                100f
            );
            GL.LoadMatrix(ref perspective);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Настройка камеры
            Matrix4 lookAt = Matrix4.LookAt(
                new OpenTK.Vector3(3, 2, 4),
                new OpenTK.Vector3(0, 0, 0),
                new OpenTK.Vector3(0, 1, 0)
            );
            GL.LoadMatrix(ref lookAt);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            // Восстанавливаем матрицу камеры
            float aspect = Width / (float)Height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                aspect,
                0.1f,
                100f
            );
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 lookAt = Matrix4.LookAt(
                new OpenTK.Vector3(3, 2, 4),
                new OpenTK.Vector3(0, 0, 0),
                new OpenTK.Vector3(0, 1, 0)
            );
            GL.LoadMatrix(ref lookAt);

            mobiusStrip.Draw();

            SwapBuffers();
        }
    }
}