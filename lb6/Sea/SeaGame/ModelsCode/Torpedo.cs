using OpenTK.Mathematics;
using Battleship3D.Graphics;

namespace Battleship3D.ModelsCode;

public class Torpedo : GameObject
{
    public Vector3 Direction;
    public float Speed = 60f;

    public Torpedo(
        Mesh mesh,
        int texture,
        Vector3 pos,
        Vector3 dir)
    {
        Mesh = mesh;
        Texture = texture;

        Position = pos;
        Direction = dir;

        float angle = MathF.Atan2(dir.X, dir.Z);
        Rotation = new Vector3(0, MathHelper.RadiansToDegrees(angle), 0);

        Scale = new Vector3(3.8f);
    }

    public override void Update(float dt)
    {
        Position += Direction * Speed * dt;

        if (Position.Length > 400)
            Active = false;
    }
}