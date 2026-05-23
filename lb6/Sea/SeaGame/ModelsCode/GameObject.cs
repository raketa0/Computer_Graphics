using OpenTK.Mathematics;
using Mesh = Battleship3D.Graphics.Mesh;

namespace Battleship3D.ModelsCode;

public class GameObject
{
    public Vector3 Position;

    public Vector3 Rotation;

    public Vector3 Scale =
        Vector3.One;

    public Mesh Mesh;

    public int Texture;

    public bool Active = true;

    public virtual void Update(float dt)
    {
    }

    public Matrix4 GetModelMatrix()
    {
        return
            Matrix4.CreateScale(Scale) *

            Matrix4.CreateRotationX(
                MathHelper.DegreesToRadians(
                    Rotation.X)) *

            Matrix4.CreateRotationY(
                MathHelper.DegreesToRadians(
                    Rotation.Y)) *

            Matrix4.CreateRotationZ(
                MathHelper.DegreesToRadians(
                    Rotation.Z)) *

            Matrix4.CreateTranslation(
                Position);
    }
}