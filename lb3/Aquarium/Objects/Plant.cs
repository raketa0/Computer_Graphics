
using OpenTK.Graphics.OpenGL;

class Plant
{
    private float x;
    private float height;

    public Plant(float x, float height)
    {
        this.x = x;
        this.height = height;
    }

    public void Draw()
    {
        GL.LineWidth(6f);
        GL.Color3(0, 0.7f, 0.2f);

        GL.Begin(PrimitiveType.LineStrip);

        for (int i = 0; i < 40; i++)
        {
            float t = i / 40f;
            float y = -1 + t * height;
            float dx = (float)System.Math.Sin(i * 0.3f) * 0.02f;

            GL.Vertex2(x + dx, y);
        }

        GL.End();
    }
}