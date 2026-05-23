using Battleship3D;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace SeaBattle3D;
public class Program
{
    public static void Main()
    {
        using Game game = new();

        game.Run();
    }
}