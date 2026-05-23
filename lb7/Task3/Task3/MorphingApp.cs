using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Task3
{
    public class MorphingApp : GameWindow
    {
        private Shader _shader = null!;
        private Shape _shape = null!;

        private float _progress;
        private float _direction = 1.0f;
        private const float MorphSpeed = 0.5f;

        private float _angleX = 0.0f;
        private float _angleY = 0.0f;
        private float _scale = 1.0f;

        private Vector2 _lastMousePos;
        private bool _isDragging;
        private float _lastWheelValue;

        public MorphingApp()
            : base(1400, 1200, GraphicsMode.Default, "app",
                  GameWindowFlags.Default, DisplayDevice.Default,
                  3, 3, GraphicsContextFlags.ForwardCompatible)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.15f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            string shaderDir = FindShadersDirectory();
            _shader = new Shader(
                Path.Combine(shaderDir, "vertex.vert"),
                Path.Combine(shaderDir, "fragment.frag")
            );

            _shape = new Shape();
        }

        private string FindShadersDirectory()
        {
            string[] possibleDirs = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Shaders"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Shaders"),
                "Shaders"
            };

            foreach (var dir in possibleDirs)
            {
                if (Directory.Exists(dir))
                    return dir;
            }

            throw new DirectoryNotFoundException("Shaders folder not found!");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Keyboard.GetState().IsKeyDown(Key.Escape))
                Exit();

            _progress += _direction * MorphSpeed * (float)e.Time;
            if (_progress >= 1.0f)
            {
                _progress = 1.0f;
                _direction = -1.0f;
            }
            else if (_progress <= 0.0f)
            {
                _progress = 0.0f;
                _direction = 1.0f;
            }

            MouseState mouseState = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);

            if (mouseState.IsButtonDown(MouseButton.Left))
            {
                if (!_isDragging)
                {
                    _isDragging = true;
                    _lastMousePos = mousePos;
                }
                else
                {
                    float dx = mousePos.X - _lastMousePos.X;
                    float dy = mousePos.Y - _lastMousePos.Y;
                    _angleY += dx * 0.5f;
                    _angleX += dy * 0.5f;
                    _lastMousePos = mousePos;
                }
            }
            else
            {
                _isDragging = false;
            }

            float wheelDelta = mouseState.Wheel - _lastWheelValue;
            if (Math.Abs(wheelDelta) > 0.001f)
            {
                _scale += wheelDelta * 0.001f;
                _scale = MathHelper.Clamp(_scale, 0.1f, 10.0f);
            }
            _lastWheelValue = mouseState.Wheel;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();

            Matrix4 model = Matrix4.CreateScale(_scale) *
                            Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_angleX)) *
                            Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_angleY));

            Matrix4 view = Matrix4.LookAt(
                new Vector3(0, 0, 4),
                Vector3.Zero,
                Vector3.UnitY);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                (float)Width / Height,
                0.1f, 100f);

            _shader.SetMatrix4("model", ref model);
            _shader.SetMatrix4("view", ref view);
            _shader.SetMatrix4("projection", ref projection);
            _shader.SetFloat("progress", _progress);

            _shape.Draw();

            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            _shape?.Dispose();
            _shader?.Dispose();
            base.OnUnload(e);
        }
    }
}