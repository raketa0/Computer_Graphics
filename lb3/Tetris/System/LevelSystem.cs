using System;

namespace Tetris.System
{
    public class LevelSystem
    {
        public int Level { get; private set; } = 1;
        public int LinesToNext { get; private set; } = 10;

        public void AddLines(int lines)
        {
            LinesToNext -= lines;

            if (LinesToNext <= 0)
            {
                Level++;
                LinesToNext = 10 + Level * 2;
            }
        }

        public float GetFallDelay()
        {
            return Math.Max(0.1f, 0.8f - Level * 0.05f);
        }

        public void Reset()
        {
            Level = 1;
            LinesToNext = 10;
        }
    }
}