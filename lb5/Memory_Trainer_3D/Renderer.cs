using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

public class Renderer
{
    int backTex;
    int[] frontTex = new int[6];

    public Renderer()
    {

        for (int i = 0; i < 6; i++)
        {
            frontTex[i] = LoadTex($"Textures/{i}.png");
        }
    }

    public void Draw(Board b)
    {
        GL.Enable(EnableCap.Texture2D);
        GL.Enable(EnableCap.CullFace);

        foreach (var t in b.Tiles)
        {
            if (t.Scale <= 0)
            {
                continue; 
            }

            GL.PushMatrix();

            GL.Translate(t.Position);
            GL.Scale(t.Scale, t.Scale, t.Scale);
            GL.Rotate(t.RotationY, 1, 0, 0);

            GL.CullFace(CullFaceMode.Back);
            GL.BindTexture(TextureTarget.Texture2D, frontTex[t.TextureId]);
            DrawQuad();

            GL.CullFace(CullFaceMode.Front);
            GL.BindTexture(TextureTarget.Texture2D, backTex);
            GL.PushMatrix();
            GL.Rotate(180, 0, 1, 0);
            DrawQuad();
            GL.PopMatrix();

            GL.PopMatrix();
        }

        GL.Disable(EnableCap.CullFace);
        GL.Disable(EnableCap.Texture2D);
    }

    void DrawQuad()
    {
        GL.Begin(PrimitiveType.Quads);

        GL.TexCoord2(0, 0); GL.Vertex3(-0.5f, 0, -0.5f);
        GL.TexCoord2(1, 0); GL.Vertex3(0.5f, 0, -0.5f);
        GL.TexCoord2(1, 1); GL.Vertex3(0.5f, 0, 0.5f);
        GL.TexCoord2(0, 1); GL.Vertex3(-0.5f, 0, 0.5f);

        GL.End();
    }

    int LoadTex(string path)
    {
        int tex = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, tex);

        using (var bmp = new Bitmap(path))
        {
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
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        return tex;
    }
}