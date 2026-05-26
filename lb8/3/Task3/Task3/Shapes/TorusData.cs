using OpenTK.Mathematics;

namespace Task3;

public class TorusData
{
    public Vector3 Center;
    public float R; // major
    public float r; // minor
    public Vector3 Color;
    public Matrix4 Transform;
    public Matrix4 InvTransform;

    public TorusData(Vector3 c, float R, float r, Vector3 color)
    {
        Center = c;
        this.R = R;
        this.r = r;
        Color = color;
        Transform = Matrix4.Identity;
        InvTransform = Matrix4.Identity;
    }

    public TorusData(Vector3 c, float R, float r, Vector3 color, Matrix4 transform)
    {
        Center = c;
        this.R = R;
        this.r = r;
        Color = color;
        Transform = transform;
        InvTransform = transform.Inverted();
    }
}