using OpenTK.Mathematics;

namespace Task1;

public class RayTracer
{
    public int W = 600;
    public int H = 400;

    public List<Sphere> Scene = new();
    public Light Light;

    public Vector3 Cam = new(0, 0, 5);
    public Vector3 Forward = new(0, 0, -1);
    public Vector3 Right = new(1, 0, 0);
    public Vector3 Up = new(0, 1, 0);

    public RayTracer()
    {
        Light = new Light(
            new Vector3(2, 3, 2),
            new Vector3(0.2f, 0.2f, 0.2f),
            new Vector3(0.8f, 0.8f, 0.8f),
            new Vector3(1.0f, 1.0f, 1.0f)
        );
    }

    public byte[] Render()
    {
        byte[] img = new byte[W * H * 3];
        int i = 0;

        float aspect = W / (float)H;

        for (int y = 0; y < H; y++)
        {
            for (int x = 0; x < W; x++)
            {
                float u = (x / (float)W * 2 - 1) * aspect;
                float v = (y / (float)H * 2 - 1);

                Vector3 rd = Vector3.Normalize(Forward + Right * u + Up * v);
                Vector3 c = Trace(Cam, rd);

                img[i++] = (byte)(Clamp(c.X) * 255);
                img[i++] = (byte)(Clamp(c.Y) * 255);
                img[i++] = (byte)(Clamp(c.Z) * 255);
            }
        }
        return img;
    }

    Vector3 Trace(Vector3 ro, Vector3 rd)
    {
        float best = float.MaxValue;
        Sphere? hit = null;

        foreach (var s in Scene)
        {
            if (SphereIntersection.Hit(ro, rd, s, out float d))
            {
                if (d < best)
                {
                    best = d;
                    hit = s;
                }
            }
        }

        if (hit == null)
            return new Vector3(0.05f, 0.05f, 0.1f);

        Vector3 p = ro + rd * best;
        return PhongLighting(p, rd, hit);
    }

    Vector3 PhongLighting(Vector3 p, Vector3 rd, Sphere sphere)
    {
        Material m = sphere.Material;
        Vector3 n = SphereIntersection.GetNormal(p, sphere);
        Vector3 l = Vector3.Normalize(Light.Position - p);
        Vector3 v = Vector3.Normalize(Cam - p);
        Vector3 r = SphereIntersection.Reflect(-l, n);

        Vector3 ambient = Light.Ambient * m.Ambient;

        float diff = MathF.Max(0.0f, Vector3.Dot(n, l));
        Vector3 diffuse = Light.Diffuse * m.Diffuse * diff;

        float spec = MathF.Pow(MathF.Max(0.0f, Vector3.Dot(r, v)), m.Shininess);
        Vector3 specular = Light.Specular * m.Specular * spec;

        return ambient + diffuse + specular;
    }

    float Clamp(float v)
    {
        if (v < 0) return 0;
        if (v > 1) return 1;
        return v;
    }
}