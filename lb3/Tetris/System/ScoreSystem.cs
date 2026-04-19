namespace Tetris.System
{
    public class ScoreSystem
    {
        public int Score { get; private set; }

        public void Add(int lines)
        {
            Score += lines switch
            {
                1 => 10,
                2 => 30,
                3 => 70,
                4 => 150,
                _ => 0
            };
        }

        public void AddLevelBonus(int emptyCells)
        {
            Score += emptyCells * 10;
        }
    }
}