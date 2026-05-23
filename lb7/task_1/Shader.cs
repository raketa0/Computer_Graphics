using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace CanabolaApp
{
    public class Shader : IDisposable
    {
        public int Handle;

        public Shader(string vertPath, string fragPath)
        {
            string baseDir = AppContext.BaseDirectory;

            string vert = File.ReadAllText(Path.Combine(baseDir, vertPath));
            string frag = File.ReadAllText(Path.Combine(baseDir, fragPath));

            int vShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vShader, vert);
            GL.CompileShader(vShader);
            Check(vShader);

            int fShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fShader, frag);
            GL.CompileShader(fShader);
            Check(fShader);

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vShader);
            GL.AttachShader(Handle, fShader);
            GL.LinkProgram(Handle);

            GL.DeleteShader(vShader);
            GL.DeleteShader(fShader);
        }

        private void Check(int shader)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
            if (status == 0)
                throw new Exception(GL.GetShaderInfoLog(shader));
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
        }
    }
}