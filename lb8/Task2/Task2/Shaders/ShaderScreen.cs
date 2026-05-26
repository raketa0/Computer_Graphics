using OpenTK.Graphics.OpenGL4;

namespace Task2;

public class ShaderScreen
{
    int handle;

    public ShaderScreen()
    {
        string vertex = File.ReadAllText("screen.vert");
        string fragment = File.ReadAllText("screen.frag");

        int vs = GL.CreateShader(ShaderType.VertexShader);

        GL.ShaderSource(vs, vertex);
        GL.CompileShader(vs);

        int fs = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(fs, fragment);
        GL.CompileShader(fs);

        handle = GL.CreateProgram();

        GL.AttachShader(handle, vs);
        GL.AttachShader(handle, fs);

        GL.LinkProgram(handle);

        GL.DeleteShader(vs);
        GL.DeleteShader(fs);
    }

    public void Use()
    {
        GL.UseProgram(handle);
    }

    public void SetFloat(string name, float value)
    {
        GL.Uniform1(
            GL.GetUniformLocation(handle, name),
            value
        );
    }

    public void SetVector2(string name, float x, float y)
    {
        GL.Uniform2(
            GL.GetUniformLocation(handle, name),
            x,
            y
        );
    }
}