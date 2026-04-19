using Tetris.System;
using Tetris.Tetromino;

namespace Tetris
{
    public class Tetris_Game
    {
        private readonly Field.Field _field = new();
        private readonly TetrominoFactory _factory = new();
        private ScoreSystem _score = new();  
        private LevelSystem _level = new();  

        private Tetris.Tetromino.Tetromino _current;
        private Tetris.Tetromino.Tetromino _next;

        private float _timer;

        public GameState State { get; private set; } = GameState.Running;

        public Tetris_Game()
        {
            _current = _factory.Create();
            _next = _factory.Create();
        }

        public void Update(float dt)
        {
            if (State != GameState.Running)
            {
                return;
            }

            _timer += dt;

            if (_timer >= _level.GetFallDelay())
            {
                _timer = 0;

                if (!TryMove(0, 1))
                {
                    _field.PlaceTetromino(_current);

                    int lines = _field.ClearLines();
                    _score.Add(lines);
                    _level.AddLines(lines);

                    SpawnNew();
                }
            }
        }

        private void SpawnNew()
        {
            _current = _next;
            _next = _factory.Create();

            if (!_field.IsValidPosition(_current.GetBlocksCopy()))
            {
                State = GameState.GameOver;
            }
        }

        private bool TryMove(int dx, int dy)
        {
            _current.Move(dx, dy);

            if (!_field.IsValidPosition(_current.GetBlocksCopy()))
            {
                _current.Move(-dx, -dy);
                return false;
            }

            return true;
        }

        public void MoveLeft() => TryMove(-1, 0);
        public void MoveRight() => TryMove(1, 0);
        public void SoftDrop() => TryMove(0, 1);

        public void Rotate()
        {
            _current.RotateClockwise();

            if (!_field.IsValidPosition(_current.GetBlocksCopy()))
            {
                _current.Move(1, 0);

                if (!_field.IsValidPosition(_current.GetBlocksCopy()))
                {
                    _current.Move(-2, 0);

                    if (!_field.IsValidPosition(_current.GetBlocksCopy()))
                    {
                        _current.Move(1, 0);
                        _current.RotateCounterClockwise();
                    }
                }
            }
        }

        public void TogglePause()
        {
            if (State == GameState.Running)
            {
                State = GameState.Paused;
            }
            else
            {
                State = GameState.Running; 
            }
        }

        public void Restart()
        {
            _field.Clear();
            _timer = 0;

            _score = new ScoreSystem();
            _level = new LevelSystem();

            _current = _factory.Create();
            _next = _factory.Create();

            State = GameState.Running;
        }

        public Field.Field Field => _field;
        public Tetris.Tetromino.Tetromino Current => _current;
        public Tetris.Tetromino.Tetromino Next => _next;
        public int Score => _score.Score;
        public int Level => _level.Level;
        public int LinesToNext => _level.LinesToNext;
    }
}