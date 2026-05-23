using Battleship3D.Graphics;
using Battleship3D.ModelsCode;
using OpenTK.Mathematics;

namespace Battleship3D.Services;

public static class ShipSpawner
{
    public static Ship Spawn(
        Random random,
        Mesh ship1Mesh,
        Mesh ship2Mesh,
        Mesh ship3Mesh,
        int ship1Tex,
        int ship2Tex,
        int ship3Tex,
        bool fromLeft = true)
    {
        int type = random.Next(3);

        Mesh mesh;
        int tex;

        switch (type)
        {
            case 0:
                mesh = ship1Mesh;
                tex = ship1Tex;
                break;

            case 1:
                mesh = ship2Mesh;
                tex = ship2Tex;
                break;

            default:
                mesh = ship3Mesh;
                tex = ship3Tex;
                break;
        }

        float zPos;
        if (fromLeft)
        {
            zPos = -60 + random.Next(20);
        }
        else
        {
            zPos = -80 + random.Next(10);
        }

        Vector3 position;
        if (fromLeft)
        {
            position = new Vector3(
                -120, 
                1.5f,
                zPos);
        }
        else
        {
            position = new Vector3(
                120,   
                1.5f,
                zPos);
        }

        float speed = 5 + random.Next(8);

        return new Ship(mesh, tex, position, speed, fromLeft);
    }
}