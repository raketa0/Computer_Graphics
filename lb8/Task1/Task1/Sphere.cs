using OpenTK.Mathematics;

namespace Task1;

public class Sphere
{
    public Vector3 Center;
    public float Radius;
    public Material Material;

    public Sphere(Vector3 center, float radius, Material material)
    {
        Center = center;
        Radius = radius;
        Material = material;
    }
}