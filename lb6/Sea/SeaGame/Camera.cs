using OpenTK.Mathematics;

namespace Battleship3D.Core;

public class Camera
{
    public Vector3 Position = new Vector3(0, 18, 85);

    public float Pitch = -12f;

    public float Yaw = -90f;

    public Matrix4 GetViewMatrix()
    {
        Vector3 front;

        front.X =
            MathF.Cos(
                MathHelper.DegreesToRadians(Pitch)) *
            MathF.Cos(
                MathHelper.DegreesToRadians(Yaw));

        front.Y =
            MathF.Sin(
                MathHelper.DegreesToRadians(Pitch));

        front.Z =
            MathF.Cos(
                MathHelper.DegreesToRadians(Pitch)) *
            MathF.Sin(
                MathHelper.DegreesToRadians(Yaw));

        front =
            Vector3.Normalize(front);

        return Matrix4.LookAt(
            Position,
            Position + front,
            Vector3.UnitY);
    }
}