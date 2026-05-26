using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Task2;

public static class Program
{
    public static void Main()
    {
        NativeWindowSettings native = new()
        {
            Size = new Vector2i(1000, 700),
            Title = "GPU Ray Tracing"
        };

        using Window window = new(
            GameWindowSettings.Default,
            native
        );

        window.Run();
    }
}