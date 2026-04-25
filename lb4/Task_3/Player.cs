using System;
using OpenTK;

namespace Maze3D
{
    public class Player
    {
        public Vector3 Position;
        public float Rotation;
        public float Pitch;

        private const float Speed = 3.5f;
        private const float Radius = 0.15f;

        public Player(Vector3 start)
        {
            Position = start;
        }

        public void Update(float dt, Maze maze, InputState input)
        {
            float sin = (float)Math.Sin(Rotation);
            float cos = (float)Math.Cos(Rotation);

            float moveX = 0;
            float moveZ = 0;

            // W/S
            if (input.Forward > 0)
            {
                moveX += sin;
                moveZ += cos;
            }
            if (input.Forward < 0)
            {
                moveX -= sin;
                moveZ -= cos;
            }

            // A/D
            if (input.Side > 0)
            {
                moveX -= cos;
                moveZ += sin;
            }
            if (input.Side < 0)
            {
                moveX += cos;
                moveZ -= sin;
            }

            if (moveX != 0 || moveZ != 0)
            {
                moveX *= Speed * dt;
                moveZ *= Speed * dt;

                if (CanMoveTo(Position.X + moveX, Position.Z, maze))
                { 
                    Position.X += moveX; 
                }

                if (CanMoveTo(Position.X, Position.Z + moveZ, maze))
                {
                    Position.Z += moveZ;
                }
            }
        }

        private bool CanMoveTo(float x, float z, Maze maze)
        {
            int minX = (int)Math.Floor(x - Radius);
            int maxX = (int)Math.Floor(x + Radius);
            int minZ = (int)Math.Floor(z - Radius);
            int maxZ = (int)Math.Floor(z + Radius);

            for (int ix = minX; ix <= maxX; ix++)
            {
                for (int iz = minZ; iz <= maxZ; iz++)
                {
                    if (maze.IsWall(ix, iz))
                    {
                        return false;
                    }
                }
            }
                

            return true;
        }
    }
}