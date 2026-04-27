using OpenTK;

public class Tile
{
    public int TextureId;
    public Vector3 Position;

    public bool Matched;
    public float RotationY;

    float target;
    float scale = 1f;

    public bool IsRevealed => RotationY >= 90f;
    public bool IsAnimating => System.Math.Abs(RotationY - target) > 1f;

    public Tile(int id, Vector3 pos)
    {
        TextureId = id;
        Position = pos;
    }

    public void Reveal() => target = 180f;
    public void Hide() => target = 0f;
    public void Match() => Matched = true;

    public void Update(float dt)
    {
        float speed = 720f;

        float diff = target - RotationY;
        float step = speed * dt;

        if (System.Math.Abs(diff) < step)
        {
            RotationY = target;
        }
        else
        {
            RotationY += System.Math.Sign(diff) * step;
        }

        if (Matched)
        {
            scale -= dt * 2f;
            if (scale < 0)
            {
                scale = 0; 
            }
        }
    }

    public float Scale => scale;
}