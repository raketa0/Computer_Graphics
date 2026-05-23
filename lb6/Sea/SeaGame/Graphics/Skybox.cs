using OpenTK.Graphics.OpenGL4;

namespace Battleship3D.Graphics;

public class Skybox
{
    int vao;
    int vbo;
    int shader;
    int texture;

    public Skybox(int texture)
    {
        this.texture = texture;

        float[] vertices =
        {
            -1f, -1f,   0f, 0f,
             1f, -1f,   1f, 0f,
             1f,  1f,   1f, 1f,

            -1f, -1f,   0f, 0f,
             1f,  1f,   1f, 1f,
            -1f,  1f,   0f, 1f
        };

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        GL.BufferData(BufferTarget.ArrayBuffer,
            vertices.Length * sizeof(float),
            vertices,
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        shader = CreateShader("Shaders/skybox.vert", "Shaders/skybox.frag");
    }

    int CreateShader(string vertPath, string fragPath)
    {
        string vert = File.ReadAllText(vertPath);
        string frag = File.ReadAllText(fragPath);

        int vs = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vs, vert);
        GL.CompileShader(vs);

        int fs = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fs, frag);
        GL.CompileShader(fs);

        int program = GL.CreateProgram();
        GL.AttachShader(program, vs);
        GL.AttachShader(program, fs);
        GL.LinkProgram(program);

        GL.DeleteShader(vs);
        GL.DeleteShader(fs);

        return program;
    }

    public void Draw()
    {
        GL.Disable(EnableCap.DepthTest);
        GL.DepthMask(false);

        GL.UseProgram(shader);

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, texture);

        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

        GL.DepthMask(true);
        GL.Enable(EnableCap.DepthTest);
    }
}