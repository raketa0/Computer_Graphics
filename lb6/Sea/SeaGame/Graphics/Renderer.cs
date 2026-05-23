using Battleship3D.Core;
using Battleship3D.ModelsCode;
using Battleship3D.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Battleship3D.Graphics;

public class Renderer
{
    private Shader shader;

    private Battleship3D.Core.Camera camera;

    public Renderer(Battleship3D.Core.Camera camera)
    {
        this.camera = camera;

        shader = new Shader(
            "Shaders/shader.vert",
            "Shaders/shader.frag");
    }

    public void BeginFrame(int width, int height)
    {
        GL.Viewport(0, 0, width, height);

        GL.Enable(EnableCap.DepthTest);

        GL.ClearColor(
            0.52f,
            0.75f,
            0.95f,
            1f);

        GL.Clear(
            ClearBufferMask.ColorBufferBit |
            ClearBufferMask.DepthBufferBit);

        shader.Use();

        Matrix4 projection =
            Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                width / (float)height,
                0.1f,
                1000f);

        Matrix4 view =
            camera.GetViewMatrix();

        shader.SetMatrix4("projection", projection);
        shader.SetMatrix4("view", view);

        shader.SetVector3(
            "lightPos",
            new OpenTK.Mathematics.Vector3(
                0,
                100,
                50));

        shader.SetVector3(
            "viewPos",
            camera.Position);

        shader.SetInt("tex", 0);
    }

    public void DrawObject(GameObject obj)
    {
        shader.SetMatrix4(
            "model",
            obj.GetModelMatrix());

        GL.ActiveTexture(TextureUnit.Texture0);

        GL.BindTexture(
            TextureTarget.Texture2D,
            obj.Texture);

        obj.Mesh.Draw();
    }

    public void DrawWater(WaterMesh water, int texture)
    {
        shader.Use();

        shader.SetMatrix4("model",
            OpenTK.Mathematics.Matrix4.CreateTranslation(0, -2f, 0));

        GL.Disable(EnableCap.CullFace);

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, texture);

        water.Draw();
    }
}