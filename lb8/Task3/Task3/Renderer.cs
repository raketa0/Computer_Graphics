using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace RayTracer;

public class Renderer
{
    int w, h;
    byte[] pixels;

    int tex, vao, vbo, shader;

    List<ISceneObject> scene;
    Game game;

    public Renderer(int W, int H, Game g)
    {
        w = W; h = H;
        game = g;
        pixels = new byte[W * H * 4];

        InitGL();
        CreateScene();
    }

    void InitGL()
    {
        tex = GL.GenTexture();

        float[] quad =
        {
            -1,-1,0,0,
             1,-1,1,0,
             1, 1,1,1,
            -1,-1,0,0,
             1, 1,1,1,
            -1, 1,0,1
        };

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, quad.Length * 4, quad, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 16, 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 16, 8);
        GL.EnableVertexAttribArray(1);

        string vs = """
        #version 330
        layout(location=0) in vec2 p;
        layout(location=1) in vec2 t;
        out vec2 uv;
        void main(){uv=t;gl_Position=vec4(p,0,1);}
        """;

        string fs = """
        #version 330
        in vec2 uv;
        out vec4 c;
        uniform sampler2D tex;
        void main(){c=texture(tex,uv);}
        """;

        int v = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(v, vs);
        GL.CompileShader(v);

        int f = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(f, fs);
        GL.CompileShader(f);

        shader = GL.CreateProgram();
        GL.AttachShader(shader, v);
        GL.AttachShader(shader, f);
        GL.LinkProgram(shader);
    }

    void CreateScene()
    {
        scene = new List<ISceneObject>();

        Vector3[] colors =
        {
            new(1,0,0),
            new(1,0.5f,0),
            new(1,1,0),
            new(0,1,0),
            new(0.2f,0.4f,1)
        };

        float baseR = 1.8f;

        for (int i = 0; i < 5; i++)
        {
            var t = new Torus(baseR - i * 0.3f, 0.25f - i * 0.02f, colors[i]);

            t.SetTransform(Matrix4.CreateTranslation(0, i * 0.7f, 0));
            scene.Add(t);
        }
    }

    public void Render()
    {
        float yaw = game.cameraAngleX;
        float pitch = game.cameraAngleY;

        float radius = 13f;

        Vector3 target = new Vector3(0, 0.8f, 0);

        Vector3 cam = target + new Vector3(
            radius * MathF.Cos(pitch) * MathF.Cos(yaw),
            radius * MathF.Sin(pitch),
            radius * MathF.Cos(pitch) * MathF.Sin(yaw)
        );

        Vector3 forward = (target - cam).Normalized();
        Vector3 right = Vector3.Cross(forward, Vector3.UnitY).Normalized();
        Vector3 up = Vector3.Cross(right, forward).Normalized();

        float aspect = w / (float)h;

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                float px = ((x / (float)w) * 2 - 1) * aspect;
                float py = 1 - (y / (float)h) * 2;

                Vector3 dir = (forward + right * px + up * py).Normalized();
                Ray ray = new Ray(cam, dir);

                Vector3 col = Trace(ray);

                int i = (y * w + x) * 4;
                pixels[i] = (byte)(col.X * 255);
                pixels[i + 1] = (byte)(col.Y * 255);
                pixels[i + 2] = (byte)(col.Z * 255);
                pixels[i + 3] = 255;
            }

        GL.BindTexture(TextureTarget.Texture2D, tex);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, w, h, 0,
            PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.UseProgram(shader);
        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    Vector3 Trace(Ray r)
    {
        float best = float.MaxValue;
        HitInfo hit = default;
        bool ok = false;

        foreach (var o in scene)
        {
            if (o.Intersect(r, out var h))
            {
                if (h.T < best)
                {
                    best = h.T;
                    hit = h;
                    ok = true;
                }
            }
        }

        if (!ok) return new Vector3(0.15f);

        float light = MathF.Max(0, Vector3.Dot(hit.Normal, new Vector3(1, 1, 0).Normalized()));

        return hit.Color * (0.25f + 0.75f * light);
    }
}