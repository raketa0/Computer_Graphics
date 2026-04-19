
namespace Tetris.Block
{
    public class Block
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Block(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Block Shift(int dx, int dy)
        {
            return new Block(X + dx, Y + dy);
        }   
    }
}
