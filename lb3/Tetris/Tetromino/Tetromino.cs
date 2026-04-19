using Tetris.Types;

namespace Tetris.Tetromino
{
    public class Tetromino
    {
        private TetrominoType _tetromino;
        private ColorType _color;
        private readonly Block.Block[] _blocks = new Block.Block[4];

        private int _originX;
        private int _originY;

        public Tetromino(
            TetrominoType type,
            int startX,
            int startY,
            ColorType color)
        {
            _tetromino = type;
            _originX = startX;
            _originY = startY;
            _color = color;

            InitBlocks();
        }

        public Block.Block[] Blocks => _blocks;
        public ColorType Color => _color;
        public TetrominoType Type => _tetromino;

        private void InitBlocks()
        {
            switch (_tetromino)
            {
                case TetrominoType.SHAPE_I:
                    _blocks[0] = new Block.Block(_originX - 1, _originY);
                    _blocks[1] = new Block.Block(_originX, _originY);
                    _blocks[2] = new Block.Block(_originX + 1, _originY);
                    _blocks[3] = new Block.Block(_originX + 2, _originY);
                    break;

                case TetrominoType.SHAPE_O:
                    _blocks[0] = new Block.Block(_originX, _originY);
                    _blocks[1] = new Block.Block(_originX + 1, _originY);
                    _blocks[2] = new Block.Block(_originX, _originY + 1);
                    _blocks[3] = new Block.Block(_originX + 1, _originY + 1);
                    break;

                case TetrominoType.SHAPE_T:
                    _blocks[0] = new Block.Block(_originX - 1, _originY);
                    _blocks[1] = new Block.Block(_originX, _originY);
                    _blocks[2] = new Block.Block(_originX + 1, _originY);
                    _blocks[3] = new Block.Block(_originX, _originY + 1);
                    break;

                case TetrominoType.SHAPE_L:
                    _blocks[0] = new Block.Block(_originX - 1, _originY);
                    _blocks[1] = new Block.Block(_originX, _originY);
                    _blocks[2] = new Block.Block(_originX + 1, _originY);
                    _blocks[3] = new Block.Block(_originX + 1, _originY + 1);
                    break;

                case TetrominoType.SHAPE_J:
                    _blocks[0] = new Block.Block(_originX - 1, _originY + 1);
                    _blocks[1] = new Block.Block(_originX - 1, _originY);
                    _blocks[2] = new Block.Block(_originX, _originY);
                    _blocks[3] = new Block.Block(_originX + 1, _originY);
                    break;

                case TetrominoType.SHAPE_S:
                    _blocks[0] = new Block.Block(_originX, _originY);
                    _blocks[1] = new Block.Block(_originX + 1, _originY);
                    _blocks[2] = new Block.Block(_originX - 1, _originY + 1);
                    _blocks[3] = new Block.Block(_originX, _originY + 1);
                    break;

                case TetrominoType.SHAPE_Z:
                    _blocks[0] = new Block.Block(_originX - 1, _originY);
                    _blocks[1] = new Block.Block(_originX, _originY);
                    _blocks[2] = new Block.Block(_originX, _originY + 1);
                    _blocks[3] = new Block.Block(_originX + 1, _originY + 1);
                    break;
            }
        }

        public void Move(int dx, int dy)
        {
            _originX += dx;
            _originY += dy;

            foreach (var b in _blocks)
            {
                b.X += dx;
                b.Y += dy;
            }
        }

        public void RotateClockwise()
        {
            if (_tetromino == TetrominoType.SHAPE_O)
            {
                return;
            }

            foreach (var b in _blocks)
            {
                int x = b.X - _originX;
                int y = b.Y - _originY;

                int newX = y;
                int newY = -x;

                b.X = _originX + newX;
                b.Y = _originY + newY;
            }
        }

        public void RotateCounterClockwise()
        {
            if (_tetromino == TetrominoType.SHAPE_O)
            {
                return;
            }

            foreach (var b in _blocks)
            {
                int x = b.X - _originX;
                int y = b.Y - _originY;

                int newX = -y;
                int newY = x;

                b.X = _originX + newX;
                b.Y = _originY + newY;
            }
        }

        public Block.Block[] GetBlocksCopy()
        {
            var copy = new Block.Block[4];

            for (int i = 0; i < 4; i++)
            {
                copy[i] = new Block.Block(_blocks[i].X, _blocks[i].Y);
            }

            return copy;
        }
    }
}