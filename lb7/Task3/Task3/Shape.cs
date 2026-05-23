using OpenTK.Graphics.OpenGL;

namespace Task3
{
    public class Shape : IDisposable
    {
        private int _vao;
        private int _vbo;
        private int _ebo;
        private int _indexCount;

        public Shape(int rows = 100, int cols = 100)
        {
            CreateMesh(rows, cols);
        }

        private void CreateMesh(int rows, int cols)
        {
            List<float> vertices = new();
            List<int> indices = new();

            for (int y = 0; y <= rows; y++)
            {
                for (int x = 0; x <= cols; x++)
                {
                    vertices.Add((float)x / cols);
                    vertices.Add((float)y / rows);
                }
            }

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x <= cols; x++)
                {
                    int top = y * (cols + 1) + x;
                    int bottom = (y + 1) * (cols + 1) + x;
                    indices.Add(top);
                    indices.Add(bottom);
                }
            }

            _indexCount = indices.Count;

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Count * sizeof(float),
                vertices.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                indices.Count * sizeof(int),
                indices.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false,
                2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.TriangleStrip, _indexCount,
                DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            GL.DeleteVertexArray(_vao);
        }
    }
}