using OpenTK.Mathematics;
using Battleship3D.Graphics;

namespace Battleship3D.ModelsCode;

public class Ship : GameObject
{
    public float Speed;
    public bool Sinking;
    public bool FromLeft;

    public Ship(
        Mesh mesh,
        int texture,
        Vector3 pos,
        float speed,
        bool fromLeft)
    {
        Mesh = mesh;
        Texture = texture;

        Position = pos;
        Speed = speed;
        FromLeft = fromLeft;

        Scale = new Vector3(7.15f);


        if (fromLeft)
        {
            Rotation = new Vector3(0, -90, 0);  
        }
        else
        {
            Rotation = new Vector3(0, -90, 0); 
        }
    }

    public override void Update(float dt)
    {
        if (!Active)
            return;

        if (Sinking)
        {
            Position.Y -= dt * 2f;

            Rotation.X += dt * 45f;
            Rotation.Z += dt * 30f;

            if (Position.Y < -20)
                Active = false;

            return;
        }

        if (FromLeft)
            Position.X += Speed * dt;  
        else
            Position.X -= Speed * dt; 
    }
}