using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Maze3D
{
    public class Game : GameWindow
    {
        private Maze _maze;
        private Player _player;
        private Renderer _renderer;

        private bool _mouseGrabbed = true;
        private float _mouseSensitivity = 0.0025f;

        private MouseState _lastMouse;

        public Game() : base(1024, 768)
        {
            Title = "Maze";
            CursorVisible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.55f, 0.75f, 0.95f, 1f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            _maze = new Maze();
            _player = new Player(new Vector3(1.5f, 0.5f, 1.5f));
            _renderer = new Renderer();
            _renderer.LoadTextures();

            _lastMouse = Mouse.GetState();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(75f),
                Width / (float)Height,
                0.05f,
                100f);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref proj);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var kb = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (kb[Key.Escape])
            {
                _mouseGrabbed = !_mouseGrabbed;
                CursorVisible = !_mouseGrabbed;
            }

            if (_mouseGrabbed)
            {
                float dx = mouse.X - _lastMouse.X;
                float dy = mouse.Y - _lastMouse.Y;

                _player.Rotation -= dx * _mouseSensitivity;
                _player.Pitch -= dy * _mouseSensitivity;

                _player.Pitch = Math.Clamp(_player.Pitch, -1.5f, 1.5f);
            }

            _lastMouse = mouse;


            int forwardValue = 0;

            bool wPressed = kb[Key.W];
            bool sPressed = kb[Key.S];

            if (wPressed && !sPressed)
            {
                forwardValue = 1;
            }
            else if (!wPressed && sPressed)
            {
                forwardValue = -1;
            }
            else
            {
                forwardValue = 0;
            }

            int sideValue = 0;

            bool dPressed = kb[Key.D];
            bool aPressed = kb[Key.A];

            if (dPressed && !aPressed)
            {
                sideValue = 1; 
            }
            else if (!dPressed && aPressed)
            {
                sideValue = -1;
            }
            else
            {
                sideValue = 0;
            }

            InputState input = new InputState
            {
                Forward = forwardValue,
                Side = sideValue
            };

            _player.Update((float)e.Time, _maze, input);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            var eye = new Vector3(_player.Position.X, 0.6f, _player.Position.Z);

            float sinY = (float)Math.Sin(_player.Rotation);
            float cosY = (float)Math.Cos(_player.Rotation);

            float sinP = (float)Math.Sin(_player.Pitch);
            float cosP = (float)Math.Cos(_player.Pitch);

            var direction = new Vector3(
                sinY * cosP,
                sinP,
                cosY * cosP
            );

            var target = eye + direction;

            Matrix4 view = Matrix4.LookAt(eye, target, Vector3.UnitY);
            GL.LoadMatrix(ref view);

            _renderer.Render(_maze);

            SwapBuffers();
        }
    }
}