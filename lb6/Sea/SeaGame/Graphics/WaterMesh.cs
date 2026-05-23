using OpenTK.Graphics.OpenGL4;

namespace Battleship3D.Graphics;

public class WaterMesh
{
    int vao;
    int vbo;

    public WaterMesh()
    {
        float[] vertices =
        {
            -300f, 0f, -300f,     0,1,0,      0,0,
             300f, 0f, -300f,     0,1,0,      1,0,
             300f, 0f,  300f,     0,1,0,      1,1,

            -300f, 0f, -300f,     0,1,0,      0,0,
             300f, 0f,  300f,     0,1,0,      1,1,
            -300f, 0f,  300f,     0,1,0,      0,1
        };

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        GL.BufferData(BufferTarget.ArrayBuffer,
            vertices.Length * sizeof(float),
            vertices,
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);
    }

    public void Draw()
    {
        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }
}