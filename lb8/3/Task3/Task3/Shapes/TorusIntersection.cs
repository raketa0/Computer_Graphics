using OpenTK.Mathematics;

namespace Task3;

public static class TorusIntersection
{
    public static bool Hit(Vector3 ro, Vector3 rd, TorusData t, out float dist)
    {
        Vector3 localRo = Vector3.TransformPosition(ro, t.InvTransform);
        Vector3 localRd = Vector3.TransformVector(rd, t.InvTransform);

        localRo -= t.Center;

        float ox = localRo.X;
        float oy = localRo.Y;
        float oz = localRo.Z;
        float dx = localRd.X;
        float dy = localRd.Y;
        float dz = localRd.Z;

        float R = t.R;
        float r = t.r;

        float G = 4.0f * R * R * (dx * dx + dz * dz);
        float H = 8.0f * R * R * (ox * dx + oz * dz);
        float I = 4.0f * R * R * (ox * ox + oz * oz);
        float J = dx * dx + dy * dy + dz * dz;
        float K = 2.0f * (ox * dx + oy * dy + oz * dz);
        float L = ox * ox + oy * oy + oz * oz + R * R - r * r;

        float c4 = J * J;
        float c3 = 2.0f * J * K;
        float c2 = K * K + 2.0f * J * L - G;
        float c1 = 2.0f * K * L - H;
        float c0 = L * L - I;

        if (MathF.Abs(c4) < 1e-10f)
        {
            dist = float.MaxValue;
            return false;
        }

        float invC4 = 1.0f / c4;
        float a = c3 * invC4;
        float b = c2 * invC4;
        float c = c1 * invC4;
        float d = c0 * invC4;

        float[] roots = SolveQuartic(a, b, c, d);

        dist = float.MaxValue;
        bool hit = false;

        foreach (float root in roots)
        {
            if (root > 0.0001f && root < dist)
            {
                Vector3 pt = localRo + localRd * root;
                float x = pt.X, y = pt.Y, z = pt.Z;
                float temp = x * x + y * y + z * z + R * R - r * r;
                float val = temp * temp - 4.0f * R * R * (x * x + z * z);

                if (MathF.Abs(val) < 0.1f * R * R)
                {
                    Vector3 worldPt = Vector3.TransformPosition(pt + t.Center, t.Transform);
                    dist = (worldPt - ro).Length;
                    hit = true;
                }
            }
        }

        return hit;
    }

    public static Vector3 GetNormal(Vector3 p, TorusData t)
    {
        Vector3 localP = Vector3.TransformPosition(p, t.InvTransform);
        localP -= t.Center;

        float x = localP.X;
        float y = localP.Y;
        float z = localP.Z;
        float R = t.R;
        float r = t.r;

        float S = x * x + y * y + z * z + R * R - r * r;
        float nx = 4.0f * x * S - 8.0f * R * R * x;
        float ny = 4.0f * y * S;
        float nz = 4.0f * z * S - 8.0f * R * R * z;

        Vector3 localNormal = new Vector3(nx, ny, nz);
        float len = localNormal.Length;
        if (len < 1e-6f)
            return Vector3.UnitY;
        localNormal /= len;


        Vector3 worldNormal = Vector3.TransformNormal(localNormal, t.Transform);
        return Vector3.Normalize(worldNormal);
    }

    static float[] SolveQuartic(float a, float b, float c, float d)
    {
        List<float> roots = new List<float>();

        float shift = a / 4.0f;
        float a2 = a * a;

        float p = b - 3.0f * a2 / 8.0f;
        float q = c - a * b / 2.0f + a2 * a / 8.0f;
        float r = d - a * c / 4.0f + a2 * b / 16.0f - 3.0f * a2 * a2 / 256.0f;

        if (MathF.Abs(q) < 1e-10f)
        {
            float disc = p * p - 4.0f * r;
            if (disc >= 0)
            {
                float sqrtDisc = MathF.Sqrt(disc);
                float z1 = (-p + sqrtDisc) / 2.0f;
                float z2 = (-p - sqrtDisc) / 2.0f;

                if (z1 >= 0)
                {
                    float sqrtZ1 = MathF.Sqrt(z1);
                    roots.Add(sqrtZ1 - shift);
                    roots.Add(-sqrtZ1 - shift);
                }
                if (z2 >= 0)
                {
                    float sqrtZ2 = MathF.Sqrt(z2);
                    roots.Add(sqrtZ2 - shift);
                    roots.Add(-sqrtZ2 - shift);
                }
            }
            return roots.ToArray();
        }

        float A = 2.0f * p;
        float B = p * p - 4.0f * r;
        float C = -q * q;

        float[] cubicRoots = SolveCubic(A, B, C);

        float z = cubicRoots[0];
        for (int i = 1; i < cubicRoots.Length; i++)
        {
            if (cubicRoots[i] > z)
                z = cubicRoots[i];
        }

        if (z < 0)
        {
            return roots.ToArray();
        }

        float sqrtZ = MathF.Sqrt(z);

        float half1 = (p + z - q / sqrtZ) / 2.0f;
        float half2 = (p + z + q / sqrtZ) / 2.0f;

        AddQuadraticRoots(1.0f, sqrtZ, half1, shift, roots);
        AddQuadraticRoots(1.0f, -sqrtZ, half2, shift, roots);

        return roots.ToArray();
    }

    static void AddQuadraticRoots(float A, float B, float C, float shift, List<float> roots)
    {
        float disc = B * B - 4.0f * A * C;

        if (disc >= -1e-6f)
        {
            if (disc < 0) disc = 0;
            float sqrtDisc = MathF.Sqrt(disc);

            float root1 = (-B + sqrtDisc) / (2.0f * A) - shift;
            float root2 = (-B - sqrtDisc) / (2.0f * A) - shift;

            if (!float.IsNaN(root1))
                roots.Add(root1);
            if (!float.IsNaN(root2))
                roots.Add(root2);
        }
    }

    static float[] SolveCubic(float A, float B, float C)
    {
        float p = B - A * A / 3.0f;
        float q = 2.0f * A * A * A / 27.0f - A * B / 3.0f + C;

        float disc = q * q / 4.0f + p * p * p / 27.0f;

        if (disc >= 0)
        {
            float sqrtD = MathF.Sqrt(disc);
            float u = MathF.Cbrt(-q / 2.0f + sqrtD);
            float v = MathF.Cbrt(-q / 2.0f - sqrtD);
            return new float[] { u + v - A / 3.0f };
        }
        else
        {
            float r = MathF.Sqrt(-p * p * p / 27.0f);
            float phi = MathF.Acos(Math.Clamp(-q / (2.0f * r), -1.0f, 1.0f));
            r = 2.0f * MathF.Pow(-p / 3.0f, 0.5f);
            return new float[]
            {
                r * MathF.Cos(phi / 3.0f) - A / 3.0f,
                r * MathF.Cos((phi + 2.0f * MathF.PI) / 3.0f) - A / 3.0f,
                r * MathF.Cos((phi + 4.0f * MathF.PI) / 3.0f) - A / 3.0f
            };
        }
    }
}