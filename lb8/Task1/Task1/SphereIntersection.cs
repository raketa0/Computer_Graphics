using OpenTK.Mathematics;

namespace Task1;

public static class SphereIntersection
{
    public static bool Hit(Vector3 ro, Vector3 rd, Sphere sphere, out float dist)
    {
        Vector3 oc = ro - sphere.Center;
        float a = Vector3.Dot(rd, rd);
        float b = 2.0f * Vector3.Dot(oc, rd);
        float c = Vector3.Dot(oc, oc) - sphere.Radius * sphere.Radius;
        float discriminant = b * b - 4.0f * a * c;

        if (discriminant < 0)
        {
            dist = float.MaxValue;
            return false;
        }

        float sqrtD = MathF.Sqrt(discriminant);
        float t1 = (-b - sqrtD) / (2.0f * a);
        float t2 = (-b + sqrtD) / (2.0f * a);

        if (t1 > 0.0001f)
            dist = t1;
        else if (t2 > 0.0001f)
            dist = t2;
        else
        {
            dist = float.MaxValue;
            return false;
        }

        return true;
    }

    public static Vector3 GetNormal(Vector3 p, Sphere sphere)
    {
        return Vector3.Normalize(p - sphere.Center);
    }

    // Отражение вектора: r = i - 2 * (i·n) * n
    public static Vector3 Reflect(Vector3 i, Vector3 n)
    {
        return i - 2.0f * Vector3.Dot(i, n) * n;
    }
}