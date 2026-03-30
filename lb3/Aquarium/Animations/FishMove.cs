using OpenTK;

namespace Aquarium.Objects
{
    public class FishMove
    {
        private Vector2 position;
        private float speed;
        private float direction;

        private float time;
        private float swayAmplitude;
        private float swayFrequency;

        public Vector2 Position => position;
        public float Flip => direction;

        public FishMove(Vector2 startPos, float speed, float direction,
                        float swayAmplitude, float swayFrequency)
        {
            position = startPos;
            this.speed = speed;
            this.direction = direction;
            this.swayAmplitude = swayAmplitude;
            this.swayFrequency = swayFrequency;
        }

        public void Update(float dt)
        {
            time += dt;

            position.X += direction * speed * dt;
            position.Y += MathF.Sin(time * swayFrequency) * swayAmplitude;

            if (position.X > 0.9f)
            {
                direction = -1f;
            }
            else if (position.X < -0.9f)
            {
                direction = 1f;
            }
        }
    }
}