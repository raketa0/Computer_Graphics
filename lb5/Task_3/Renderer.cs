using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Maze3D
{
    public class Renderer
    {
        private int[] _wallTextures = new int[7]; // 1..6

        public void LoadTextures()
        {
            _wallTextures[1] = LoadTexture("textures/1.png");
            _wallTextures[2] = LoadTexture("textures/2.png");
            _wallTextures[3] = LoadTexture("textures/3.png");
            _wallTextures[4] = LoadTexture("textures/4.png");
            _wallTextures[5] = LoadTexture("textures/5.png");
            _wallTextures[6] = LoadTexture("textures/6.png");
        }

        private int LoadTexture(string path)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            using var bmp = new Bitmap(path);
            var data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba,
                data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);



            return id;
        }

        public void Render(Maze maze)
        {
            DrawFloorAndCeiling(maze);
            DrawWalls(maze);
        }

        private void DrawFloorAndCeiling(Maze maze)
        {
            GL.Disable(EnableCap.Texture2D);

            // Пол
            GL.Color3(0.4f, 0.3f, 0.2f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(maze.Size, 0, 0);
            GL.Vertex3(maze.Size, 0, maze.Size);
            GL.Vertex3(0, 0, maze.Size);
            GL.End();

            // Потолок
            GL.Color3(0.75f, 0.75f, 0.85f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(0, 1, 0);
            GL.Vertex3(maze.Size, 1, 0);
            GL.Vertex3(maze.Size, 1, maze.Size);
            GL.Vertex3(0, 1, maze.Size);
            GL.End();

            GL.Enable(EnableCap.Texture2D);
        }

        private void DrawWalls(Maze maze)
        {
            for (int x = 0; x < maze.Size; x++)
            {
                for (int z = 0; z < maze.Size; z++)
                {
                    int type = maze.GetWallType(x, z);
                    if (type != 0)
                    {
                        DrawCube(x, z, type);
                    }
                }
            }
        }

        private void DrawCube(int x, int z, int type)
        {
            GL.BindTexture(TextureTarget.Texture2D, _wallTextures[type]);

            float h = 1f;

            GL.Begin(PrimitiveType.Quads);

            // Перед
            GL.TexCoord2(0, 0); 
            GL.Vertex3(x, 0, z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(x + 1, 0, z);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x + 1, h, z);
            GL.TexCoord2(0, 1); 
            GL.Vertex3(x, h, z);

            // Зад
            GL.TexCoord2(0, 0);
            GL.Vertex3(x, 0, z + 1);
            GL.TexCoord2(1, 0);
            GL.Vertex3(x + 1, 0, z + 1);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x + 1, h, z + 1);
            GL.TexCoord2(0, 1);
            GL.Vertex3(x, h, z + 1);

            // Лево
            GL.TexCoord2(0, 0);
            GL.Vertex3(x, 0, z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(x, 0, z + 1);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x, h, z + 1);
            GL.TexCoord2(0, 1); 
            GL.Vertex3(x, h, z);

            // Право
            GL.TexCoord2(0, 0);
            GL.Vertex3(x + 1, 0, z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(x + 1, 0, z + 1);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x + 1, h, z + 1);
            GL.TexCoord2(0, 1); 
            GL.Vertex3(x + 1, h, z);

            GL.End();
        }
    }
}