using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Aquarium.Objects
{
    class Bubble
    {
        private Vector2 pos;

        public Bubble(Vector2 start)
        {
            pos = start;
        }

        public void Update(float dt)
        {
            pos.Y += dt * 0.25f;
        }

        public float GetY()
        {
            return pos.Y;
        }

        public void Draw()
        {
            GL.Color4(0.8f, 0.9f, 1f, 0.5f);
            GL.LineWidth(3f);
            GL.Begin(PrimitiveType.LineLoop);

            for (int i = 0; i < 20; i++)
            {
                double a = i * Math.PI * 2 / 20;

                GL.Vertex2(
                    pos.X + Math.Cos(a) * 0.02,
                    pos.Y + Math.Sin(a) * 0.02
                );
            }

            GL.End();
        }
    }
}