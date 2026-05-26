
using OpenTK.Windowing.Desktop;

namespace Task1;

public static class Program
{
    public static void Main()
    {
        try
        {
            var native = new NativeWindowSettings
            {
                Size = new OpenTK.Mathematics.Vector2i(900, 700),
                Title = "Phong Lighting - Spheres"
            };

            using Window window = new Window(GameWindowSettings.Default, native);
            window.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}