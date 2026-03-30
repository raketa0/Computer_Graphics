using OpenTK;
using OpenTK.Input;

namespace MouseController
{
    public class MouseController
    {
        private Vector2[] points;
        private int selectedPoint = -1;
        private float radius = 0.7f;

        public MouseController(Vector2[] controlPoints)
        {
            points = controlPoints;
        }

        public void MouseDown(MouseButtonEventArgs e, int width, int height)
        {
            Vector2 mouse = ScreenToWorld(
                new Vector2(e.Position.X, e.Position.Y),
                width,
                height);

            for (int i = 0; i < points.Length; i++)
            {
                if ((points[i] - mouse).Length < radius)
                {
                    selectedPoint = i;
                    break;
                }
            }
        }

        public void MouseUp()
        {
            selectedPoint = -1;
        }

        public void MouseMove(MouseMoveEventArgs e, int width, int height)
        {
            if (selectedPoint == -1)
            {
                return;
            }
            Vector2 mouse = ScreenToWorld(
                new Vector2(e.Position.X, e.Position.Y),
                width,
                height);

            points[selectedPoint] = mouse;
        }

        private Vector2 ScreenToWorld(Vector2 mouse, int width, int height)
        {
            float x = (mouse.X / width) * 40f - 20f;
            float y = -((mouse.Y / height) * 20f - 10f);

            return new Vector2(x, y);
        }
    }
}