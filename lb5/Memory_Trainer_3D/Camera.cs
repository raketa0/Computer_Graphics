using OpenTK;

public class Camera
{
    public Vector3 Position = new Vector3(6, 8, 12);

    public Matrix4 GetView()
    {
        return Matrix4.LookAt(Position, new Vector3(3, 0, 2), Vector3.UnitY);
    }

    public Matrix4 GetProjection(int w, int h)
    {
        return Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(40),
            w / (float)h,
            0.1f,
            100);
    }
}