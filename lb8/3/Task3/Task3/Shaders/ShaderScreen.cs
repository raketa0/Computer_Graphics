using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace Task3;

public class ShaderScreen
{
    int _handle;

    public ShaderScreen()
    {
        string v = File.ReadAllText("screen.vert");
        string f = File.ReadAllText("screen.frag");

        int vs = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vs, v);
        GL.CompileShader(vs);

        int fs = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fs, f);
        GL.CompileShader(fs);

        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vs);
        GL.AttachShader(_handle, fs);
        GL.LinkProgram(_handle);

        GL.DeleteShader(vs);
        GL.DeleteShader(fs);
    }

    public void Use()
    {
        GL.UseProgram(_handle);
        GL.Uniform1(GL.GetUniformLocation(_handle, "tex"), 0);
    }
}