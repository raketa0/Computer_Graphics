using Aquarium.Objects;
using OpenTK;
using OpenTK.Graphics;

namespace Primitive.Scene
{
    class Aquarium
    {
        private List<Fish> fishes = new();
        private List<Plant> plants = new();
        private List<Stone> stones = new();
        private List<Bubble> bubbles = new();

        private Background bg = new();
        private Sand sand = new();

        public Aquarium()
        {
            fishes.Add(new Fish(new Vector2(-0.8f, 0.3f), Color4.Orange, 1.0f, FishType.OVAL_FISH, 0.11f, 1f, 0.009f, 2f, 0f));
            fishes.Add(new Fish(new Vector2(-0.5f, -0.2f), Color4.Gold, 0.9f, FishType.OVAL_FISH, 0.18f, 1f, 0.0015f, 1.5f, 0.5f));
            fishes.Add(new Fish(new Vector2(0.2f, 0.4f), Color4.OrangeRed, 1.1f, FishType.OVAL_FISH, 0.25f, -1f, 0.0025f, 2.5f, 1f));

            fishes.Add(new Fish(new Vector2(0.7f, -0.3f), Color4.Cyan, 0.8f, FishType.RECTANGE_FISH, 0.20f, -1f, 0.0018f, 1.8f, 1.5f));
            fishes.Add(new Fish(new Vector2(-0.1f, 0.5f), Color4.LightBlue, 1.0f, FishType.RECTANGE_FISH, 0.16f, 1f, 0.0022f, 2.2f, 0.8f));
            fishes.Add(new Fish(new Vector2(0.5f, 0.2f), Color4.Blue, 0.85f, FishType.RECTANGE_FISH, 0.21f, -1f, 0.0017f, 1.7f, 1.2f));

            fishes.Add(new Fish(new Vector2(-0.6f, -0.45f), Color4.Yellow, 1.05f, FishType.TRIANGLE_FISH, 0.13f, 1f, 0.013f, 1.3f, 0.3f));
            fishes.Add(new Fish(new Vector2(0.3f, -0.4f), Color4.LightPink, 0.9f, FishType.TRIANGLE_FISH, 0.19f, -1f, 0.0019f, 1.9f, 1.3f));
            fishes.Add(new Fish(new Vector2(0.0f, 0.0f), Color4.Violet, 1.1f, FishType.TRIANGLE_FISH, 0.24f, 1f, 0.0024f, 2.4f, 0.7f));

            for (int i = 0; i < 80; i++)
            {
                plants.Add(new Plant(
                    -1f + i * 0.025f,
                    0.8f + (i % 6) * 0.2f
                ));
            }

            for (int i = 0; i < 60; i++)
            {
                float x = -1f + i * 0.035f;

                float y;
                if (i % 3 == 0)
                {
                    y = -0.87f;
                }
                else if (i % 3 == 1)
                {
                    y = -0.92f;
                }
                else
                {
                    y = -0.95f;
                }

                stones.Add(new Stone(
                    new Vector2(x, y),
                    0.045f + (i % 5) * 0.01f,
                    GetStoneColor(i)
                ));
            }
        }

        private Color4 GetStoneColor(int i)
        {
            if (i % 4 == 0) 
            {
                return Color4.Gray;
            }
            if (i % 4 == 1)
            {
                return Color4.DarkGray;
            }
            if (i % 4 == 2)
            {
                return Color4.LightGray;
            }
            return Color4.Beige;
        }

        public void Update(float dt)
        {
            foreach (var f in fishes)
            {
                f.Update(dt, bubbles);
            }

            foreach (var b in bubbles)
            {
                b.Update(dt);
            }

            bubbles.RemoveAll(b => b.GetY() > 1);
        }

        public void Draw()
        {
            bg.Draw();
            sand.Draw();

            foreach (var plant in plants)
            {
                plant.Draw();
            }

            foreach (var stone in stones)
            {
                stone.Draw();
            }

            foreach (var fish in fishes)
            {
                fish.Draw();
            }

            foreach (var bubble in bubbles)
            {
                bubble.Draw();
            }
        }
    }
}