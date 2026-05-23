using Assimp;
using OpenTK.Graphics.OpenGL4;
using PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;

namespace Battleship3D.Graphics;

public class Mesh
{
    public int VAO;

    int VBO;

    public int VertexCount;

    public Mesh(string path)
    {
        AssimpContext importer = new();

        Scene scene =
            importer.ImportFile(
                path,
                PostProcessSteps.Triangulate |
                PostProcessSteps.GenerateNormals);

        List<float> vertices = new();

        foreach (var mesh in scene.Meshes)
        {
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                vertices.Add(mesh.Vertices[i].X);
                vertices.Add(mesh.Vertices[i].Y);
                vertices.Add(mesh.Vertices[i].Z);

                vertices.Add(mesh.Normals[i].X);
                vertices.Add(mesh.Normals[i].Y);
                vertices.Add(mesh.Normals[i].Z);

                if (mesh.HasTextureCoords(0))
                {
                    vertices.Add(mesh.TextureCoordinateChannels[0][i].X);
                    vertices.Add(mesh.TextureCoordinateChannels[0][i].Y);
                }
                else
                {
                    vertices.Add(0);
                    vertices.Add(0);
                }
            }
        }

        VertexCount = vertices.Count / 8;

        VAO = GL.GenVertexArray();

        VBO = GL.GenBuffer();

        GL.BindVertexArray(VAO);

        GL.BindBuffer(
            BufferTarget.ArrayBuffer,
            VBO);

        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertices.Count * sizeof(float),
            vertices.ToArray(),
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(
            0,
            3,
            VertexAttribPointerType.Float,
            false,
            8 * sizeof(float),
            0);

        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(
            1,
            3,
            VertexAttribPointerType.Float,
            false,
            8 * sizeof(float),
            3 * sizeof(float));

        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(
            2,
            2,
            VertexAttribPointerType.Float,
            false,
            8 * sizeof(float),
            6 * sizeof(float));

        GL.EnableVertexAttribArray(2);
    }

    public void Draw()
    {
        GL.BindVertexArray(VAO);

        GL.DrawArrays(
            PrimitiveType.Triangles,
            0,
            VertexCount);
    }
}