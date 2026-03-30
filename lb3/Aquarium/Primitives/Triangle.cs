using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Aquarium.Primitives
{
    class Triangle : IPrimitive
    {
        private Vector2 a, b, c;
        private Color4 color;

        public Triangle(Vector2 a, Vector2 b, Vector2 c, Color4 color)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.color = color;
        }

        public List<Vector2> GetPosition()
        {
            return new List<Vector2> { a, b, c };
        }

        public void Move(Vector2 delta)
        {
            a += delta;
            b += delta;
            c += delta;
        }

        public void Draw()
        {
            GL.Color4(color);
            GL.Begin(PrimitiveType.Triangles);

            GL.Vertex2(a);
            GL.Vertex2(b);
            GL.Vertex2(c);

            GL.End();
        }
    }
}