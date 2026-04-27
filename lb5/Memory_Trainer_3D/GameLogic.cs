public class GameLogic
{
    Tile first, second;
    float timer;

    public void OnClick(Tile t)
    {
        if (t.Matched)
        {
            return; 
        }

        if (t.IsRevealed)
        {
            t.Hide();
            return;
        }

        if (first == null)
        {
            first = t;
            t.Reveal();
        }
        else if (second == null && t != first)
        {
            second = t;
            t.Reveal();
            timer = 1f;
        }
    }

    public void Update(float dt)
    {
        if (second == null)
        {
            return; 
        }

        timer -= dt;

        if (timer <= 0)
        {
            if (first.TextureId == second.TextureId)
            {
                first.Match();
                second.Match();
            }
            else
            {
                first.Hide();
                second.Hide();
            }

            first = null;
            second = null;
        }
    }
}