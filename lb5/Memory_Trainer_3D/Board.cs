using OpenTK;
using System;
using System.Linq;
using System.Collections.Generic;

public class Board
{
    public Tile[,] Tiles;

    public Board(int r, int c)
    {
        Tiles = new Tile[r, c];

        var ids = new List<int>();

        for (int i = 0; i < 6; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        var rnd = new Random();
        ids = ids.OrderBy(x => rnd.Next()).ToList();

        int k = 0;
        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
            {
                Tiles[i, j] = new Tile(ids[k++], new Vector3(j * 1.3f, 0, i * 1.3f));
            }
        }
            
    }

    public void Update(float dt)
    {
        foreach (var t in Tiles)
        {
            t.Update(dt);
        }
    }

    public bool IsAnimating()
    {
        foreach (var t in Tiles)
        {
            if (t.IsAnimating)
            {
                return true;
            }
        }
        return false;
    }
}