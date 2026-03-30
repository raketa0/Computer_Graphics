using OpenTK;
using OpenTK.Graphics;
using Aquarium.Primitives;
using OpenTK.Graphics.OpenGL;

namespace Aquarium.Objects
{
    public enum FishType
    {
        OVAL_FISH,
        RECTANGE_FISH,
        TRIANGLE_FISH
    }

    public enum FishPartType
    {
        BODY,
        TAIL,
        FIN,
        MOUTH,
        EYE
    }

    class Fish
    {
        private List<(FishPartType type, IPrimitive primitive)> parts = new();

        private FishMove movement;
        private float size;
        private FishType type;

        private float bubbleTimer;
        private float bubbleInterval = 2f;

        public Vector2 Position => movement.Position;
        public float Flip => movement.Flip;

        public Fish(Vector2 pos, Color4 color, float size, FishType type,
                    float speed, float direction,
                    float swayAmplitude, float swayFrequency,
                    float bubbleOffset)
        {
            this.size = size;
            this.type = type;

            movement = new FishMove(pos, speed, direction, swayAmplitude, swayFrequency);

            bubbleTimer = bubbleOffset;

            CreateFish(color);
        }

        private void CreateFish(Color4 color)
        {
            float s = size;

            switch (type)
            {
                case FishType.OVAL_FISH:
                    parts.Add((FishPartType.BODY,
                        new Ellipse(Vector2.Zero, 0.095f * s, 0.058f * s, color)));

                    parts.Add((FishPartType.TAIL,
                        new Triangle(
                            new Vector2(-0.09f * s, 0),
                            new Vector2(-0.175f * s, 0.072f * s),
                            new Vector2(-0.175f * s, -0.072f * s), color)));

                    parts.Add((FishPartType.FIN,
                        new Triangle(
                            new Vector2(-0.065f * s, 0.035f * s),
                            new Vector2(0.025f * s, 0.082f * s),
                            new Vector2(0.055f * s, 0.038f * s), color)));

                    parts.Add((FishPartType.MOUTH,
                        new Triangle(
                            new Vector2(0.085f * s, 0.008f * s),
                            new Vector2(0.115f * s, 0),
                            new Vector2(0.085f * s, -0.008f * s), color)));
                    break;

                case FishType.RECTANGE_FISH:
                    parts.Add((FishPartType.BODY,
                        new Rectangle(Vector2.Zero, 0.095f * s, 0.055f * s, color)));

                    parts.Add((FishPartType.TAIL,
                        new Triangle(
                            new Vector2(-0.095f * s, 0),
                            new Vector2(-0.19f * s, 0.085f * s),
                            new Vector2(-0.19f * s, -0.085f * s), color)));

                    parts.Add((FishPartType.FIN,
                        new Triangle(
                            new Vector2(-0.04f * s, 0.055f * s),
                            new Vector2(0.055f * s, 0.025f * s),
                            new Vector2(-0.04f * s, 0.105f * s), color)));

                    parts.Add((FishPartType.MOUTH,
                        new Rectangle(
                            new Vector2(0.1f * s, 0), 0.018f * s, 0.008f * s, color)));
                    break;

                case FishType.TRIANGLE_FISH:
                    parts.Add((FishPartType.BODY,
                        new Triangle(
                            new Vector2(0.105f * s, 0),
                            new Vector2(-0.075f * s, 0.075f * s),
                            new Vector2(-0.075f * s, -0.075f * s), color)));

                    parts.Add((FishPartType.TAIL,
                        new Triangle(
                            new Vector2(-0.075f * s, 0),
                            new Vector2(-0.135f * s, 0.070f * s),
                            new Vector2(-0.135f * s, -0.070f * s), color)));

                    parts.Add((FishPartType.MOUTH,
                        new Ellipse(
                            new Vector2(0.105f * s, 0), 0.005f * s, 0.005f * s, color)));
                    break;
            }

            AddEye(s);
        }

        private void AddEye(float s)
        {
            Vector2 eye = new Vector2(0.06f * s, 0.02f * s);

            parts.Add((FishPartType.EYE,
                new Ellipse(eye, 0.019f * s, 0.019f * s, Color4.White)));

            parts.Add((FishPartType.EYE,
                new Ellipse(eye + new Vector2(0.006f * s, 0),
                0.0095f * s, 0.0095f * s, Color4.Black)));
        }

        public void Update(float dt, List<Bubble> bubbles)
        {
            movement.Update(dt);

            Vector2 current = parts[0].primitive.GetPosition()[0];
            Vector2 delta = movement.Position - current;

            foreach (var part in parts)
            {
                part.primitive.Move(delta);
            }

            bubbleTimer += dt;

            if (bubbleTimer > bubbleInterval)
            {
                bubbles.Add(new Bubble(GetMouthWorldPosition()));
                bubbleTimer = 0f;
            }
        }

        public void Draw()
        {
            GL.PushMatrix();

            if (Flip < 0)
            {
                Vector2 center = parts[0].primitive.GetPosition()[0];

                GL.Translate(center.X, center.Y, 0);
                GL.Scale(-1f, 1f, 1f);
                GL.Translate(-center.X, -center.Y, 0);
            }

            foreach (var part in parts)
            {
                part.primitive.Draw();
            }

            GL.PopMatrix();
        }

        public Vector2 GetMouthWorldPosition()
        {
            foreach (var part in parts)
            {
                if (part.type == FishPartType.MOUTH)
                {
                    return part.primitive.GetPosition()[0];
                }
            }

            return movement.Position;
        }
    }
}