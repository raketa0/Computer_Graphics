using Assimp;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;
using TextureWrapMode = OpenTK.Graphics.OpenGL.TextureWrapMode;

namespace Task1.Models
{
    public class Shape
    {
        private Scene _scene = null!;
        private readonly Dictionary<Mesh, int> _meshDisplayLists = new();
        private float _x;
        private float _y;
        private float _z;
        private readonly float _scale;
        private float[] _color = new float[] { 1f, 1f, 1f };
        private bool _hasColor;
        private bool _hasTexture;
        private int _textureId;
        private bool _isBoard;

        public float X
        {
            get => _x;
            set => _x = value;
        }

        public float Y
        {
            get => _y;
            set => _y = value;
        }

        public float Z
        {
            get => _z;
            set => _z = value;
        }

        public Shape(float x, float y, float z, float scale)
        {
            _x = x;
            _y = y;
            _z = z;
            _scale = scale;
        }

        public void LoadPicture(string path)
        {
            AssimpContext context = new AssimpContext();
            _scene = context.ImportFile(path,
                PostProcessSteps.Triangulate |
                PostProcessSteps.GenerateNormals |
                PostProcessSteps.CalculateTangentSpace |
                PostProcessSteps.FlipUVs
            );

            _isBoard = path.Contains("Board.3ds");

            string? modelDirectory = Path.GetDirectoryName(path);
            if (_isBoard && modelDirectory != null)
            {
                foreach (Material material in _scene.Materials)
                {
                    LoadMaterialTexture(material, modelDirectory);
                }
            }

            foreach (Mesh mesh in _scene.Meshes)
            {
                CreateDisplayList(mesh);
            }
        }

        private void CreateDisplayList(Mesh mesh)
        {
            int displayList = GL.GenLists(1);
            GL.NewList(displayList, ListMode.Compile);

            Vector3[] vertices = mesh.Vertices
                .Select(v => new Vector3(v.X, v.Y, v.Z))
                .ToArray();

            Vector3[] normals = mesh.Normals
                .Select(n => new Vector3(n.X, n.Y, n.Z))
                .ToArray();

            Vector2[] texCoords = mesh.TextureCoordinateChannels[0]?
                .Select(t => new Vector2(t.X, t.Y))
                .ToArray() ?? Array.Empty<Vector2>();

            int[] indices = mesh.GetIndices();

            // Включаем текстуру если нужно
            if (_hasTexture && _textureId != 0)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, _textureId);
                // Включаем автоматическое наложение текстурных координат
                GL.Enable(EnableCap.TextureGenS);
                GL.Enable(EnableCap.TextureGenT);
            }

            GL.Begin(PrimitiveType.Triangles);

            for (int i = 0; i < indices.Length; i++)
            {
                int index = indices[i];

                // Нормали
                if (normals.Length > index)
                {
                    GL.Normal3(normals[index]);
                }

                // Текстурные координаты
                if (_hasTexture && texCoords.Length > index)
                {
                    GL.TexCoord2(texCoords[index].X, texCoords[index].Y);
                }

                // Вершины
                GL.Vertex3(vertices[index].X * _scale,
                          vertices[index].Y * _scale,
                          vertices[index].Z * _scale);
            }

            GL.End();

            if (_hasTexture && _textureId != 0)
            {
                GL.Disable(EnableCap.TextureGenS);
                GL.Disable(EnableCap.TextureGenT);
                GL.Disable(EnableCap.Texture2D);
            }

            GL.EndList();

            _meshDisplayLists[mesh] = displayList;
        }

        private void LoadMaterialTexture(Material material, string modelDirectory)
        {
            if (material.HasTextureDiffuse)
            {
                TextureSlot textureSlot = material.TextureDiffuse;
                string texturePath = textureSlot.FilePath;

                // Ищем файл текстуры
                string fullPath = Path.Combine(modelDirectory, texturePath);
                if (!File.Exists(fullPath))
                {
                    fullPath = Path.Combine(modelDirectory, Path.GetFileName(texturePath));
                }

                if (!File.Exists(fullPath))
                {
                    fullPath = Path.Combine(Directory.GetCurrentDirectory(), "textures", Path.GetFileName(texturePath));
                }

                if (!File.Exists(fullPath))
                {
                    fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "textures", Path.GetFileName(texturePath));
                }

                if (File.Exists(fullPath))
                {
                    _textureId = LoadTexture(fullPath);
                    _hasTexture = true;
                    Console.WriteLine($"Texture loaded: {fullPath}, ID: {_textureId}");
                }
                else
                {
                    Console.WriteLine($"Texture not found: {texturePath}");
                }
            }
        }

        private int LoadTexture(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return 0;
            }

            try
            {
                // Используем System.Drawing для загрузки текстуры
                using (Bitmap bitmap = new Bitmap(filePath))
                {
                    // Переворачиваем изображение по вертикали (OpenGL использует нижний левый угол как начало координат)
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    int textureId = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, textureId);

                    // Получаем данные пикселей
                    BitmapData data = bitmap.LockBits(
                        new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    // Загружаем текстуру
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                        bitmap.Width, bitmap.Height, 0,
                        PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                    bitmap.UnlockBits(data);

                    // Настройки текстуры
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                        (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                        (int)TextureMagFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                        (int)TextureWrapMode.Repeat);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                        (int)TextureWrapMode.Repeat);

                    // Включаем авто-генерацию текстурных координат если нужно
                    GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode,
                        (int)TextureEnvMode.Modulate);

                    Console.WriteLine($"Texture loaded successfully: {filePath} (ID: {textureId}, Size: {bitmap.Width}x{bitmap.Height})");
                    return textureId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texture {filePath}: {ex.Message}");
                return 0;
            }
        }

        public void Dispose()
        {
            foreach (int displayList in _meshDisplayLists.Values)
            {
                GL.DeleteLists(displayList, 1);
            }
            if (_hasTexture && _textureId != 0)
            {
                GL.DeleteTexture(_textureId);
            }
        }

        public void SetColor(float[] color)
        {
            _color = color;
            _hasColor = true;
        }

        public void Paint()
        {
            GL.PushMatrix();
            GL.Translate(_x, _y, _z);

            // Устанавливаем цвет
            if (_hasColor && !_hasTexture)
            {
                GL.Color3(_color[0], _color[1], _color[2]);
            }
            else if (_hasColor && _hasTexture)
            {
                // Для текстур используем белый цвет, чтобы текстура отображалась правильно
                GL.Color3(1f, 1f, 1f);
            }

            // Рисуем все меши
            foreach (Mesh mesh in _scene.Meshes)
            {
                int displayList = _meshDisplayLists[mesh];
                GL.CallList(displayList);
            }

            GL.PopMatrix();
        }
    }
}