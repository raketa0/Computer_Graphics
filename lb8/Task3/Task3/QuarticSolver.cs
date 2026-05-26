using System;
using System.Collections.Generic;

namespace RayTracer;

public static class QuarticSolver
{
    public static float[] Solve(float a, float b, float c, float d, float e)
    {
        if (Math.Abs(a) < 1e-8f)
            return Array.Empty<float>();

        b /= a; c /= a; d /= a; e /= a;

        float bb = b * b;

        float p = c - 3f * bb / 8f;
        float q = d + bb * b / 8f - b * c / 2f;
        float r = e - 3f * bb * bb / 256f + bb * c / 16f - b * d / 4f;

        List<float> roots = new();

        if (Math.Abs(q) < 1e-6f)
        {
            var quad = Quad(1, p, r);
            foreach (var y in quad)
            {
                if (y < 0) continue;
                float s = MathF.Sqrt(y);
                roots.Add(s - b / 4f);
                roots.Add(-s - b / 4f);
            }
            return roots.ToArray();
        }

        var cubic = Cubic(1, 2 * p, p * p - 4 * r, -q * q);
        if (cubic.Length == 0) return Array.Empty<float>();

        float z = cubic[0];
        float u = MathF.Sqrt(MathF.Max(z, 0));

        if (u < 1e-6f) return Array.Empty<float>();

        float s1 = -(2 * p + z + q / u);
        float s2 = -(2 * p + z - q / u);

        foreach (var x in Quad(1, u, s1 / 2)) roots.Add(x - b / 4f);
        foreach (var x in Quad(1, -u, s2 / 2)) roots.Add(x - b / 4f);

        return roots.ToArray();
    }

    static float[] Quad(float a, float b, float c)
    {
        float d = b * b - 4 * a * c;
        if (d < 0) return Array.Empty<float>();

        float s = MathF.Sqrt(d);
        return new[]
        {
            (-b + s) / (2*a),
            (-b - s) / (2*a)
        };
    }

    static float[] Cubic(float a, float b, float c, float d)
    {
        b /= a; c /= a; d /= a;

        float p = c - b * b / 3f;
        float q = 2 * b * b * b / 27f - b * c / 3f + d;

        float D = q * q / 4f + p * p * p / 27f;

        if (D > 0)
        {
            float u = Cube(-q / 2f + MathF.Sqrt(D));
            float v = Cube(-q / 2f - MathF.Sqrt(D));
            return new[] { u + v - b / 3f };
        }

        float phi = MathF.Acos(-q / (2 * MathF.Sqrt(-(p * p * p) / 27f)));
        float t = 2 * MathF.Sqrt(-p / 3f);

        return new[]
        {
            t * MathF.Cos(phi/3f) - b/3f
        };
    }

    static float Cube(float x)
        => x >= 0 ? MathF.Pow(x, 1f / 3f) : -MathF.Pow(-x, 1f / 3f);
}