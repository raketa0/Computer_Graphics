using Tetris;
using Tetris.Rendering;

class Program
{
    static void Main()
    {
        using var game = new Game();
        game.Run(60.0);
    }
}