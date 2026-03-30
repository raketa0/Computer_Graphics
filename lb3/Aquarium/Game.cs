using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Primitive.Scene;

namespace Primitive
{
    class Game : GameWindow
    {
        private Primitive.Scene.Aquarium aquarium;

        public Game() : base(800, 600, GraphicsMode.Default, "Aquarium")
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 1);
            aquarium = new Primitive.Scene.Aquarium();
        }

        protected override void OnResize(System.EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            float aspect = Width / (float)Height;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            if (aspect >= 1)
            {
                GL.Ortho(-aspect, aspect, -1, 1, -1, 1);
            }
            else
            {
                GL.Ortho(-1, 1, -1 / aspect, 1 / aspect, -1, 1);
            }

            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            aquarium.Update((float)e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();

            aquarium.Draw();

            SwapBuffers();
        }
    }
}