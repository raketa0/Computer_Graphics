using OpenTK.Graphics.OpenGL;

namespace Maze3D
{
    public class Renderer
    {
        public void Render(Maze maze)
        {
            DrawFloorAndCeiling(maze);
            DrawWalls(maze);
        }

        private void DrawFloorAndCeiling(Maze maze)
        {
            GL.Color3(0.4f, 0.3f, 0.2f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(maze.Size, 0, 0);
            GL.Vertex3(maze.Size, 0, maze.Size);
            GL.Vertex3(0, 0, maze.Size);
            GL.End();

            GL.Color3(0.75f, 0.75f, 0.85f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(0, 1, 0);
            GL.Vertex3(maze.Size, 1, 0);
            GL.Vertex3(maze.Size, 1, maze.Size);
            GL.Vertex3(0, 1, maze.Size);
            GL.End();
        }

        private void DrawWalls(Maze maze)
        {
            for (int x = 0; x < maze.Size; x++)
            {
                for (int z = 0; z < maze.Size; z++)
                {
                    if (maze.IsWall(x, z))
                    {
                        DrawCube(x, z);
                    }
                }
            }
                
        }

        private void DrawCube(int x, int z)
        {
            GL.Color3(0.3f, 0.3f, 0.3f);

            float h = 1f;

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(x, 0, z);
            GL.Vertex3(x + 1, 0, z);
            GL.Vertex3(x + 1, h, z);
            GL.Vertex3(x, h, z);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(x, 0, z + 1);
            GL.Vertex3(x + 1, 0, z + 1);
            GL.Vertex3(x + 1, h, z + 1);
            GL.Vertex3(x, h, z + 1);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(x, 0, z);
            GL.Vertex3(x, 0, z + 1);
            GL.Vertex3(x, h, z + 1);
            GL.Vertex3(x, h, z);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(x + 1, 0, z);
            GL.Vertex3(x + 1, 0, z + 1);
            GL.Vertex3(x + 1, h, z + 1);
            GL.Vertex3(x + 1, h, z);
            GL.End();

            GL.Color3(0.5f, 0.2f, 0.1f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(x, h, z);
            GL.Vertex3(x + 1, h, z);
            GL.Vertex3(x + 1, h, z + 1);
            GL.Vertex3(x, h, z + 1);
            GL.End();
        }
    }
}