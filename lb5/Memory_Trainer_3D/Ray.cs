using OpenTK;

public struct Ray
{
    public Vector3 Origin;
    public Vector3 Direction;

    public Ray(Vector3 o, Vector3 d)
    {
        Origin = o;
        Direction = d;
    }
}