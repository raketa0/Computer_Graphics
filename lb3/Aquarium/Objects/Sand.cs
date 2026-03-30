using OpenTK.Graphics.OpenGL;

namespace Aquarium.Objects
{
    class Sand
    {
        public void Draw()
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(0.8f, 0.7f, 0.5f);
            GL.Vertex2(-2, -1);

            GL.Color3(0.8f, 0.7f, 0.5f);
            GL.Vertex2(2, -1);

            GL.Color3(0.6f, 0.5f, 0.3f);
            GL.Vertex2(2, -0.8f);

            GL.Color3(0.6f, 0.5f, 0.3f);
            GL.Vertex2(-2, -0.8f);

            GL.End();
        }
    }
}