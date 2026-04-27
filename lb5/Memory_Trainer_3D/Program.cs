using OpenTK;

class Program
{
    static void Main()
    {
        using var game = new Game(1000, 800);
        game.Run(60);
    }
}