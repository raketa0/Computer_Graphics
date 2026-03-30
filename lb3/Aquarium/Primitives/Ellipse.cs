using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Aquarium.Primitives
{
    class Ellipse : IPrimitive
    {
        private Vector2 center;
        private float radiusX;
        private float radiusY;
        private Color4 color;

        public Ellipse(Vector2 center, float radius, Color4 color)
            : this(center, radius, radius, color)
        {
        }

        public Ellipse(Vector2 center, float radiusX, float radiusY, Color4 color)
        {
            this.center = center;
            this.radiusX = Math.Max(radiusX, 0.001f);
            this.radiusY = Math.Max(radiusY, 0.001f);
            this.color = color;
        }

        public void Move(Vector2 delta)
        {
            center += delta;
        }

        public List<Vector2> GetPosition()
        {
            return new List<Vector2> { center };
        }

        public void Draw()
        {
            GL.Color4(color);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(center.X, center.Y);

            const int segments = 48;

            for (int i = 0; i <= segments; i++)
            {
                double angle = i * Math.PI * 2.0 / segments;
                float x = center.X + (float)Math.Cos(angle) * radiusX;
                float y = center.Y + (float)Math.Sin(angle) * radiusY;
                GL.Vertex2(x, y);
            }

            GL.End();
        }
    }
}