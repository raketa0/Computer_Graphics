using OpenTK;

public class Camera
{
    public Vector3 Position = new Vector3(1.95f, 10, 10f);

    public Matrix4 GetView()
    {
        return Matrix4.LookAt(Position, new Vector3(1.95f, 0, 1.3f), Vector3.UnitY);
    }

    public Matrix4 GetProjection(int w, int h)
    {
        return Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(40),
            w / (float)h,
            0.1f,
            70);
    }
}