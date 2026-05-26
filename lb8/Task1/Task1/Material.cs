using OpenTK.Mathematics;

namespace Task1;

public struct Material
{
    public Vector3 Ambient;
    public Vector3 Diffuse;
    public Vector3 Specular;
    public float Shininess;

    public Material(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
    {
        Ambient = ambient;
        Diffuse = diffuse;
        Specular = specular;
        Shininess = shininess;
    }
}