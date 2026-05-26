using OpenTK.Mathematics;

namespace RayTracer;

public struct HitInfo
{
    public float T;
    public Vector3 Position;
    public Vector3 Normal;
    public Vector3 Color;
}