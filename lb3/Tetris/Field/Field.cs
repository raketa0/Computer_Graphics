using Tetris.Block;
using Tetris.Tetromino;

namespace Tetris.Field
{
    public class Field
    {
        public int Width { get; } = 10;
        public int Height { get; } = 20;

        private readonly int[,] _grid;

        public Field()
        {
            _grid = new int[Height, Width];
        }

        public void Clear()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _grid[y, x] = 0;
        }

        public bool IsValidPosition(Block.Block[] blocks)
        {
            foreach (var b in blocks)
            {
                if (b.Y < 0)
                {
                    continue;
                }

                if (b.X < 0 || b.X >= Width || b.Y >= Height)
                {
                    return false;
                }

                if (_grid[b.Y, b.X] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void PlaceTetromino(Tetromino.Tetromino tetromino)
        {
            foreach (var b in tetromino.Blocks)
            {
                if (b.Y < 0)
                {
                    continue;
                }

                _grid[b.Y, b.X] = (int)tetromino.Color + 1;
            }
        }

        public int ClearLines()
        {
            int cleared = 0;

            for (int y = Height - 1; y >= 0; y--)
            {
                bool full = true;

                for (int x = 0; x < Width; x++)
                {
                    if (_grid[y, x] == 0)
                    {
                        full = false;
                        break;
                    }
                }

                if (full)
                {
                    RemoveLine(y);
                    cleared++;
                    y++;
                }
            }

            return cleared;
        }

        private void RemoveLine(int line)
        {
            for (int y = line; y > 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    _grid[y, x] = _grid[y - 1, x];
                }
            }
            for (int x = 0; x < Width; x++)
            {
                _grid[0, x] = 0;
            }
        }


        public int GetEmptyLinesCount()
        {
            int emptyLines = 0;

            for (int y = 0; y < Height; y++)
            {
                bool empty = true;
                for (int x = 0; x < Width; x++)
                {
                    if (_grid[y, x] != 0)
                    {
                        empty = false;
                        break;
                    }
                }
                if (empty)
                {
                    emptyLines++;
                }
            }

            return emptyLines;
        }

        public int GetCell(int x, int y) => _grid[y, x];

        public bool IsValid(Block.Block[] blocks)
        {
            foreach (var b in blocks)
            {
                if (b.Y >= 0)
                {
                    if (b.X < 0 || b.X >= Width || b.Y >= Height)
                    {
                        return false;
                    }

                    if (_grid[b.Y, b.X] != 0)
                    {
                        return false;
                    }

                }
            }

            return true;
        }
    }
}