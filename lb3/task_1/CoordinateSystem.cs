using OpenTK.Graphics.OpenGL;

namespace CoordinateSystem
{
    public class CoordinateSystem
    {
        public void Draw()
        {
            DrawAxes();
            DrawTicks();
        }

        private void DrawAxes()
        {
            GL.Color3(0f, 0f, 0f);

            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(-20, 0);
            GL.Vertex2(20, 0);

            GL.Vertex2(20, 0);
            GL.Vertex2(19, 0.5);

            GL.Vertex2(20, 0);
            GL.Vertex2(19, -0.5);

            GL.Vertex2(0, -10);
            GL.Vertex2(0, 10);

            GL.Vertex2(0, 10);
            GL.Vertex2(0.5, 9);

            GL.Vertex2(0, 10);
            GL.Vertex2(-0.5, 9);

            GL.End();
        }

        private void DrawTicks()
        {
            GL.LineWidth(1f);
            GL.Color3(0.5f, 0.5f, 0.5f);

            GL.Begin(PrimitiveType.Lines);

            for (int x = -20; x <= 20; x++)
            {
                if (x == 0)
                {
                    continue;
                }
                GL.Vertex2(x, -0.2f);
                GL.Vertex2(x, 0.2f);
            }

            for (int y = -10; y <= 10; y++)
            {
                if (y == 0)
                {
                    continue;
                }
                GL.Vertex2(-0.2f, y);
                GL.Vertex2(0.2f, y);
            }

            GL.End();
        }
    }
}