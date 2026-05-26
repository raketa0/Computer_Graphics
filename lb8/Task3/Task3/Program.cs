using System;

namespace RayTracer
{
    internal static class Program
    {
        static void Main()
        {
            using Game game = new Game(1280, 720);
            game.Run();
        }
    }
}