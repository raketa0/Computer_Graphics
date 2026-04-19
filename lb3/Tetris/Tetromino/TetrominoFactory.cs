using Tetris.Types;

namespace Tetris.Tetromino
{
    public class TetrominoFactory
    {
        private readonly Random _random = new();

        public Tetromino Create()
        {
            var type = (TetrominoType)_random.Next(0, 7);
            var color = GetRandomColor();

            return new Tetromino(type, 5, 0, color);
        }

        private ColorType GetRandomColor()
        {
            var colors = new[]
            {
                ColorType.WHITE,
                ColorType.YELLOW,
                ColorType.PURPLE,
                ColorType.ORANGE,
                ColorType.BLUE,
                ColorType.GREEN,
                ColorType.RED
            };

            return colors[_random.Next(colors.Length)];
        }
    }
}