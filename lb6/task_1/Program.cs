using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ChessGame game = new ChessGame())
            {
                game.Run(60.0);
            }
        }
    }
}