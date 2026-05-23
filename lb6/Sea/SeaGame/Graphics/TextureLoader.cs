using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Battleship3D.Services;

public static class TextureLoader
{
    public static int Load(string path)
    {
        int texture =
            GL.GenTexture();

        GL.BindTexture(
            TextureTarget.Texture2D,
            texture);

        using (Stream stream = File.OpenRead(path))
        {
            ImageResult image =
                ImageResult.FromStream(
                    stream,
                    ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                image.Data);
        }

        GL.GenerateMipmap(
            GenerateMipmapTarget.Texture2D);

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapS,
            (int)TextureWrapMode.Repeat);

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapT,
            (int)TextureWrapMode.Repeat);

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.LinearMipmapLinear);

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Linear);

        return texture;
    }
}