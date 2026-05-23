using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace CanabolaApp
{
    public class Game : GameWindow
    {
        private int _vao;
        private int _vbo;
        private Shader _shader;

        private float[] _data;

        public Game()
            : base(800, 600,
                GraphicsMode.Default,
                "Canabola",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, 3,
                GraphicsContextFlags.ForwardCompatible)
        { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.05f, 0.05f, 0.07f, 1f);

            _shader = new Shader("shader.vert", "shader.frag");

            GenerateLine();

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer,
                _data.Length * sizeof(float),
                _data,
                BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 1, VertexAttribPointerType.Float, false, sizeof(float), 0);

            GL.BindVertexArray(0);
        }
        // формулу понять
        private void GenerateLine()
        {
            var list = new List<float>();

            float step = (float)Math.PI / 100f;

            for (float x = 0; x <= Math.PI * 2; x += step)
                list.Add(x);

            _data = list.ToArray();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();

            int scaleLoc = GL.GetUniformLocation(_shader.Handle, "scale");
            GL.Uniform1(scaleLoc, 0.35f);

            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.LineStrip, 0, _data.Length);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Keyboard.GetState().IsKeyDown(Key.Escape))
                Exit();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            GL.DeleteBuffer(_vbo);
            GL.DeleteVertexArray(_vao);
            _shader.Dispose();
        }
    }
}