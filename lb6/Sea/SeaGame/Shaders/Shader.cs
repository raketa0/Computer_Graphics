using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Battleship3D.Shaders;

public class Shader
{
    int handle;

    public Shader(
        string vertexPath,
        string fragmentPath)
    {
        string vertexShaderSource =
            File.ReadAllText(vertexPath);

        string fragmentShaderSource =
            File.ReadAllText(fragmentPath);

        int vertexShader =
            GL.CreateShader(
                ShaderType.VertexShader);

        GL.ShaderSource(
            vertexShader,
            vertexShaderSource);

        GL.CompileShader(
            vertexShader);

        int fragmentShader =
            GL.CreateShader(
                ShaderType.FragmentShader);

        GL.ShaderSource(
            fragmentShader,
            fragmentShaderSource);

        GL.CompileShader(
            fragmentShader);

        handle =
            GL.CreateProgram();

        GL.AttachShader(
            handle,
            vertexShader);

        GL.AttachShader(
            handle,
            fragmentShader);

        GL.LinkProgram(handle);

        GL.DeleteShader(vertexShader);

        GL.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        GL.UseProgram(handle);
    }

    public void SetMatrix4(string name, Matrix4 matrix)
    {
        int location = GL.GetUniformLocation(handle, name);
        GL.UniformMatrix4(location, false, ref matrix);
    }

    public void SetVector3(
        string name,
        Vector3 vector)
    {
        int location =
            GL.GetUniformLocation(
                handle,
                name);

        GL.Uniform3(
            location,
            vector);
    }

    public void SetInt(
        string name,
        int value)
    {
        int location =
            GL.GetUniformLocation(
                handle,
                name);

        GL.Uniform1(
            location,
            value);
    }
}