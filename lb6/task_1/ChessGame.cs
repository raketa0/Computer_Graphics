using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using Task1.Models.Data;
using Task1.Services;

namespace Task1
{
    public class ChessGame : GameWindow
    {
        private ChessPainter _chessPainter = null!;
        private bool _isDragging;
        private Vector2 _lastMousePos;
        private Vector3 _cameraPos = new Vector3(0, 20f, 15f);
        private MouseState _lastMouseState;

        public ChessGame() : base(1500, 800, OpenTK.Graphics.GraphicsMode.Default, "3D Chess Game")
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Normalize);

            float[] lightPos = { 5.0f, 10.0f, 5.0f, 1.0f };
            float[] lightAmbient = { 0.3f, 0.3f, 0.3f, 1.0f };
            float[] lightDiffuse = { 0.8f, 0.8f, 0.8f, 1.0f };

            GL.Light(LightName.Light0, LightParameter.Position, lightPos);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightDiffuse);

            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);

            _chessPainter = new ChessPainter(GetChessMoves());
        }

        protected override void OnUnload(EventArgs e)
        {
            _chessPainter.Dispose();
            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                Width / (float)Height,
                0.1f,
                100f);
            GL.LoadMatrix(ref perspective);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _chessPainter.Update((float)e.Time);

            // Обновляем состояние мыши
            _lastMouseState = Mouse.GetState();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            Vector3 lookAt = new Vector3(0, 0, 0);
            Matrix4 lookAtMatrix = Matrix4.LookAt(_cameraPos, lookAt, Vector3.UnitZ);
            GL.LoadMatrix(ref lookAtMatrix);

            _chessPainter.Paint();

            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Left)
            {
                MouseState currentState = Mouse.GetState();
                _lastMousePos = new Vector2(currentState.X, currentState.Y);
                _isDragging = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButton.Left)
            {
                _isDragging = false;
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (!_isDragging)
                return;

            MouseState currentState = Mouse.GetState();
            float dx = (_lastMousePos.X - currentState.X) / 20f;
            _lastMousePos = new Vector2(currentState.X, currentState.Y);
            _cameraPos = RotateVector(_cameraPos, dx);
        }

        private static Vector3 RotateVector(Vector3 vector, float angle)
        {
            float x = vector.X;
            float y = vector.Y;

            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            return new Vector3(x * cos - y * sin, x * sin + y * cos, vector.Z);
        }

        private static ChessMove[] GetChessMoves()
        {
            return new ChessMove[]
            {
                new ChessMove(4, 1, 4, 3),
                new ChessMove(4, 6, 4, 4),
                new ChessMove(5, 0, 2, 3),
                new ChessMove(1, 7, 2, 5),
                new ChessMove(3, 0, 7, 4),
                new ChessMove(6, 7, 5, 5),
                new ChessMove(7, 4, 5, 6)
            };
        }
    }
}