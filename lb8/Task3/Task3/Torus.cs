using OpenTK.Mathematics;

namespace RayTracer;

public class Torus : ISceneObject
{
    public float R;
    public float r;
    public Vector3 Color;

    public Matrix4 Transform = Matrix4.Identity;
    public Matrix4 Inv = Matrix4.Identity;

    public Torus(float R, float r, Vector3 c)
    {
        this.R = R;
        this.r = r;
        Color = c;
    }

    public void SetTransform(Matrix4 m)
    {
        Transform = m;
        Inv = m.Inverted();
    }

    float F(Vector3 p)
    {
        float x2z2 = p.X * p.X + p.Z * p.Z;
        float s = x2z2 + p.Y * p.Y + R * R - r * r;
        return s * s - 4 * R * R * x2z2;
    }

    Vector3 Normal(Vector3 p)
    {
        float e = 0.001f;

        float dx = F(p + new Vector3(e, 0, 0)) - F(p - new Vector3(e, 0, 0));
        float dy = F(p + new Vector3(0, e, 0)) - F(p - new Vector3(0, e, 0));
        float dz = F(p + new Vector3(0, 0, e)) - F(p - new Vector3(0, 0, e));

        return new Vector3(dx, dy, dz).Normalized();
    }

    public bool Intersect(Ray ray, out HitInfo hit)
    {
        hit = default;

        Vector3 ro = Vector3.TransformPosition(ray.Origin, Inv);
        Vector3 rd = Vector3.TransformNormal(ray.Direction, Inv).Normalized();

        float ox = ro.X, oy = ro.Y, oz = ro.Z;
        float dx = rd.X, dy = rd.Y, dz = rd.Z;

        float sum = dx * dx + dy * dy + dz * dz;
        float e = ox * ox + oy * oy + oz * oz - R * R - r * r;
        float f = ox * dx + oy * dy + oz * dz;

        float A = sum * sum;
        float B = 4 * sum * f;
        float C = 2 * sum * e + 4 * f * f + 4 * R * R * dz * dz;
        float D = 4 * f * e + 8 * R * R * oz * dz;
        float E = e * e - 4 * R * R * (R * R - oz * oz);

        var roots = QuarticSolver.Solve(A, B, C, D, E);

        float best = float.MaxValue;
        bool ok = false;

        foreach (var t in roots)
        {
            if (t > 0.001f && t < best)
            {
                best = t;
                ok = true;
            }
        }

        if (!ok) return false;

        Vector3 p = ray.At(best);
        Vector3 pl = Vector3.TransformPosition(p, Inv);

        hit.T = best;
        hit.Position = p;
        hit.Normal = Vector3.TransformNormal(Normal(pl), Transform).Normalized();
        hit.Color = Color;

        return true;
    }
}