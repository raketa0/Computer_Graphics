using OpenTK.Mathematics;

namespace RayTracer;

public struct Ray
{
    public Vector3 Origin;
    public Vector3 Direction;

    public Ray(Vector3 o, Vector3 d)
    {
        Origin = o;
        Direction = d.Normalized();
    }

    public Vector3 At(float t) => Origin + Direction * t;
}