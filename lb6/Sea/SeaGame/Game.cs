using Battleship3D.Core;
using Battleship3D.Graphics;
using Battleship3D.ModelsCode;
using Battleship3D.Services;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SeaGame.Services;
using Mesh = Battleship3D.Graphics.Mesh;

namespace Battleship3D;

public class Game : GameWindow
{
    Renderer renderer;

    Camera camera = new();
    AudioService audio = new();
    List<Ship> ships = new();
    List<Torpedo> torpedoes = new();

    Mesh ship1Mesh;
    Mesh ship2Mesh;
    Mesh ship3Mesh;
    Mesh torpedoMesh;

    WaterMesh water;
    Skybox skybox;

    int ship1Tex;
    int ship2Tex;
    int ship3Tex;
    int torpedoTex;
    int waterTex;
    int skyTex;
    int shootBuffer;
    int shootSource;
    float cooldown = 0;

    int lives = 3;
    int score = 0;

    Random random = new();

    public Game()
        : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Size = new Vector2i(1280, 720),
                Title = "Battleship 3D"
            })
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.Enable(EnableCap.DepthTest);

        camera.Position =
            new Vector3(0, 22, 70);

        camera.Pitch = -15f;

        renderer = new Renderer(camera);

        ship1Mesh =
            new Mesh("models/battleship/battleship.obj");

        ship2Mesh =
            new Mesh("models/cruiser/cruiser.obj");

        ship3Mesh =
            new Mesh("models/destroyer/destroyer.obj");

        torpedoMesh =
            new Mesh("models/torpedo/torpedo.obj");

        water = new WaterMesh();

        ship1Tex =
            TextureLoader.Load(
                "models/battleship/battleship.jpg");

        ship2Tex =
            TextureLoader.Load(
                "models/cruiser/cruiser.jpg");

        ship3Tex =
            TextureLoader.Load(
                "models/destroyer/destroyer.jpg");

        torpedoTex =
            TextureLoader.Load(
                "models/torpedo/torpedo.jpg");

        waterTex =
            TextureLoader.Load(
                "models/water.jpg");

        skyTex =
            TextureLoader.Load(
                "models/sky.jpeg");

        skybox = new Skybox(skyTex);

        shootBuffer = audio.LoadSound("sounds/shoot.wav");
        shootSource = audio.CreateSource(shootBuffer);

        SpawnShip(true);  
        SpawnShip(true);
        SpawnShip(false); 
        SpawnShip(false);
    }

    void SpawnShip(bool fromLeft = true)
    {
        ships.Add(
            ShipSpawner.Spawn(
                random,
                ship1Mesh,
                ship2Mesh,
                ship3Mesh,
                ship1Tex,
                ship2Tex,
                ship3Tex,
                fromLeft));
    }

    protected override void OnUpdateFrame(
        FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        float dt = (float)args.Time;

        if (cooldown > 0)
            cooldown -= dt;

        foreach (var ship in ships)
            ship.Update(dt);

        foreach (var torpedo in torpedoes)
            torpedo.Update(dt);

        foreach (var torpedo in torpedoes.ToList())
        {
            if (!torpedo.Active)
                continue;

            foreach (var ship in ships.ToList())
            {
                if (!ship.Active || ship.Sinking)
                    continue;

                float collisionRadius = 6.5f;
                float distance = Vector3.Distance(torpedo.Position, ship.Position);

                if (distance < collisionRadius)
                {
                    ship.Sinking = true;
                    torpedo.Active = false;
                    score += 100;
                    break;
                }
            }
        }

        for (int i = ships.Count - 1; i >= 0; i--)
        {
            Ship ship = ships[i];

            if (!ship.Sinking)
            {
                if ((ship.FromLeft && ship.Position.X > 120) ||
                    (!ship.FromLeft && ship.Position.X < -120))
                {
                    ships.RemoveAt(i);
                    lives--;
                    SpawnShip(!ship.FromLeft);
                    continue;
                }
            }
            else if (!ship.Active)
            {
                ships.RemoveAt(i);
                SpawnShip(random.Next(2) == 0);
            }
        }

        torpedoes.RemoveAll(t => !t.Active);

        if (MouseState.IsButtonDown(
            OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left))
        {
            Shoot();
        }

        if (lives <= 0)
        {
            Title = $"GAME OVER | Score: {score}";
        }
        else
        {
            Title = $"Battleship 3D | Lives: {lives} | Score: {score} | Cooldown: {cooldown:F1}";
        }
    }

    void Shoot()
    {
        if (cooldown > 0)
            return;

        cooldown = 2f;

        audio.Play(shootSource);

        float mouseX = (MouseState.X / (float)Size.X) * 2.0f - 1.0f;

        Vector3 direction = new Vector3(
            mouseX * 1.5f,
            0,
            -1.0f
        );

        direction = Vector3.Normalize(direction);

        torpedoes.Add(
            new Torpedo(
                torpedoMesh,
                torpedoTex,
                new Vector3(0, -0.5f, 10),
                direction));
    }

    protected override void OnRenderFrame(
        FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        renderer.BeginFrame(
            Size.X,
            Size.Y);

        skybox.Draw();
        GL.Enable(EnableCap.DepthTest);

        renderer.DrawWater(
            water,
            waterTex);

        foreach (var ship in ships)
        {
            if (ship.Active)
                renderer.DrawObject(ship);
        }

        foreach (var torpedo in torpedoes)
        {
            if (torpedo.Active)
                renderer.DrawObject(torpedo);
        }

        SwapBuffers();
    }
}