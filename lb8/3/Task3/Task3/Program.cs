using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Task3;

public static class Program
{
    public static void Main()
    {
        var native = new NativeWindowSettings
        {
            Size = new OpenTK.Mathematics.Vector2i(900, 700),
            Title = "tor"
        };

        using var window = new Window(GameWindowSettings.Default, native);
        window.Run();
    }
}