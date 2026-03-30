using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BezierCurve
{
    public class BezierCurve
    {
        private Vector2[] controlPoints;

        public Vector2[] ControlPoints => controlPoints;

        public BezierCurve(Vector2[] points)
        {
            controlPoints = points;
        }

        public void Draw()
        {
            DrawCurve();
            DrawControlPolygon();
        }

        private void DrawCurve()
        {
            GL.Color3(1f, 0f, 0f);

            GL.Begin(PrimitiveType.LineStrip);

            for (float t = 0; t <= 1; t += 0.01f)
            {
                Vector2 p = CalculatePoint(t);
                GL.Vertex2(p.X, p.Y);
            }

            GL.End();
        }

        private void DrawControlPolygon()
        {
            GL.Enable(EnableCap.LineStipple);
            GL.LineStipple(1, 0x00FF);

            GL.Color3(0f, 0f, 1f);

            GL.Begin(PrimitiveType.LineStrip);

            foreach (var p in controlPoints)
                GL.Vertex2(p.X, p.Y);

            GL.End();

            GL.Disable(EnableCap.LineStipple);

            GL.PointSize(6);
            GL.Color3(0f, 0f, 0f);

            GL.Begin(PrimitiveType.Points);

            foreach (var p in controlPoints)
                GL.Vertex2(p.X, p.Y);

            GL.End();
        }

        private Vector2 CalculatePoint(float t)
        {
            float u = 1 - t;

            return u * u * u * controlPoints[0] +
                   3 * u * u * t * controlPoints[1] +
                   3 * u * t * t * controlPoints[2] +
                   t * t * t * controlPoints[3];
        }
    }
}