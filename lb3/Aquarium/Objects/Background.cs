using OpenTK.Graphics.OpenGL;

namespace Aquarium.Objects
{
    class Background
    {
        public void Draw()
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(0.2f, 0.5f, 0.8f);
            GL.Vertex2(-2, 1);
            GL.Vertex2(2, 1);

            GL.Color3(0.0f, 0.1f, 0.3f);
            GL.Vertex2(2, -1);
            GL.Vertex2(-2, -1);

            GL.End();
        }
    }
}