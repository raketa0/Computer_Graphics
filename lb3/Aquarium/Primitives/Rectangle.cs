using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Aquarium.Primitives
{
    class Rectangle : IPrimitive
    {
        private Vector2 pos;
        private float width, height;
        private Color4 color;

        public Rectangle(Vector2 pos, float width, float height, Color4 color)
        {
            this.pos = pos;
            this.width = width;
            this.height = height;
            this.color = color;
        }

        public List<Vector2> GetPosition()
        {
            return new List<Vector2>
            {
                new Vector2(pos.X - width, pos.Y - height),
                new Vector2(pos.X + width, pos.Y - height),
                new Vector2(pos.X + width, pos.Y + height),
                new Vector2(pos.X - width, pos.Y + height)
            };
        }


        public void Move(Vector2 delta) => pos += delta;

        public void Draw()
        {
            GL.Color4(color);
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(pos.X - width, pos.Y - height);
            GL.Vertex2(pos.X + width, pos.Y - height);
            GL.Vertex2(pos.X + width, pos.Y + height);
            GL.Vertex2(pos.X - width, pos.Y + height);

            GL.End();
        }
    }
}