using System;

namespace Galaxian3D
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run(60.0);
            }
        }
    }
}