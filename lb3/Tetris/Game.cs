using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Tetris.Rendering;
using Tetris.System;

namespace Tetris
{
    public class Game : GameWindow
    {
        private Tetris_Game _game = new();
        private Renderer _renderer = new(); 

        private double _inputTimer = 0;
        private const double InputDelay = 0.1;

        private bool _rotatePressed = false;
        private bool _pausePressed = false;

        public Game() : base(500, 600, GraphicsMode.Default, "Tetris") { }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Width, Height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Width, Height, 0, -1, 1);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var input = Keyboard.GetState();
            _inputTimer += e.Time;

            if (_inputTimer > InputDelay)
            {
                if (input.IsKeyDown(Key.Left))
                {
                    _game.MoveLeft();
                }
                if (input.IsKeyDown(Key.Right))
                {
                    _game.MoveRight();
                }
                if (input.IsKeyDown(Key.Down))
                {
                    _game.SoftDrop();
                }
                _inputTimer = 0;
            }

            if (input.IsKeyDown(Key.Up))
            {
                if (!_rotatePressed)
                {
                    _game.Rotate();
                    _rotatePressed = true;
                }
            }
            else _rotatePressed = false;

            if (input.IsKeyDown(Key.P))
            {
                if (!_pausePressed)
                {
                    _game.TogglePause();
                    _pausePressed = true;
                }
            }
            else _pausePressed = false;

            if (_game.State == GameState.GameOver && input.IsKeyDown(Key.Enter))
            {
                _game.Restart();
            }

            _game.Update((float)e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _renderer.Draw(_game);
            SwapBuffers();
        }
    }
}