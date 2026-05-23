using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace CanabolaApp
{
    class Program
    {
        static void Main()
        {
            using (var game = new Game())
                game.Run(60.0);
        }
    }
}