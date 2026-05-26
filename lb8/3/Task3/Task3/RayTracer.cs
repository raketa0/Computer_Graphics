using OpenTK.Mathematics;

namespace Task3;

public class RayTracer
{
    public int W = 600;
    public int H = 400;

    public List<Torus> Scene = new();

    public Vector3 Cam = new(0, 0, 5);
    public Vector3 Forward = new(0, 0, -1);
    public Vector3 Right = new(1, 0, 0);
    public Vector3 Up = new(0, 1, 0);

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
        Torus? hit = null;

        foreach (var t in Scene)
        {
            if (TorusIntersection.Hit(ro, rd, t.Data, out float d))
            {
                if (d < best)
                {
                    best = d;
                    hit = t;
                }
            }
        }

        if (hit == null)
            return new Vector3(0.05f, 0.05f, 0.1f);

        Vector3 p = ro + rd * best;
        return hit.Data.Color * Lighting(p, rd, hit.Data);
    }

    float Lighting(Vector3 p, Vector3 rd, TorusData t)
    {
        Vector3 lightDir = Vector3.Normalize(new Vector3(2, 3, 2));
        Vector3 n = TorusIntersection.GetNormal(p, t);

        if (Vector3.Dot(n, rd) > 0)
            n = -n;

        float diffuse = MathF.Max(0.0f, Vector3.Dot(n, lightDir));
        float ambient = 0.3f;
        return ambient + (1.0f - ambient) * diffuse;
    }

    float Clamp(float v)
    {
        if (v < 0)
            return 0;
        if (v > 1)
            return 1;
        return v;
    }
}