class Program
{
    static void Main()
    {
        using var game = new Maze3D.Game();
        game.Run(60.0);
    }
}