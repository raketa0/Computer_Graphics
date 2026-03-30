using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Aquarium.Primitives;

namespace Aquarium.Objects
{
    class Stone
    {
        private Vector2 pos;
        private float size;
        private Color4 color;

        public Stone(Vector2 pos, float size, Color4 color)
        {
            this.pos = new Vector2(pos.X, pos.Y);
            this.size = size;
            this.color = color;
        }

        public void Draw()
        {
            float scaledRadiusX = size * 1.5f;
            float scaledRadiusY = size * 0.7f;

            var ellipse = new Ellipse(pos, scaledRadiusX, scaledRadiusY, color);
            ellipse.Draw();
        }
    }
}