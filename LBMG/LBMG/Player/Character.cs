using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LBMG.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LBMG.Player
{
    public class Character
    {
        public string Name { get; set; }
        public float Speed { get; set; }
        public Direction Direction { get; set; }
        public bool IsMoving { get; set; }
        public Point Coordinates { get; private set; }
        public int MoveState { get; set; }
        public event EventHandler<Point> Moved;
        public event EventHandler<Point> Teleported;

        public Character(string name, float speed)
        {
            Name = name;
            Speed = speed;
            Direction = Direction.Bottom;
            IsMoving = false;
            Coordinates = new Point(0, 0);
            MoveState = 1;
        }

        public void SpawnAt(int x, int y/*, Map map*/)
        {
            Coordinates = new Point(x, y);
            Teleported?.Invoke(this, Coordinates);
        }

        public void Move()
        {
            Coordinates = GetFacingPoint();
            Moved?.Invoke(this, Coordinates);
            Debug.WriteLine("Player pos: " + Coordinates);
        }

        public bool EncounteredCharacter(Character otherChar, int minDistance = 5)
        {
            int deltaX = Coordinates.X - otherChar.Coordinates.X,
                deltaY = Coordinates.Y - otherChar.Coordinates.Y;

            if ((Math.Abs(deltaX) <= minDistance && deltaY == 0) || (Math.Abs(deltaY) <= minDistance && deltaX == 0))  // Like a cross
            {
                if (deltaY > 0) // Relative dir: top
                {
                    Direction = Direction.Bottom;
                    otherChar.Direction = Direction.Top;
                }
                else if (deltaY < 0)
                {
                    Direction = Direction.Top;
                    otherChar.Direction = Direction.Bottom;
                }
                else if (deltaX > 0) // Relative dir: right
                {
                    Direction = Direction.Left;
                    otherChar.Direction = Direction.Right;
                }
                else if (deltaX < 0)
                {
                    Direction = Direction.Right;
                    otherChar.Direction = Direction.Left;
                }

                return true;
            }

            return false;
        }

        public Point GetFacingPoint()
        {
            int x = Coordinates.X;
            int y = Coordinates.Y;

            x += Direction switch
            {
                Direction.Left => -1,
                Direction.Top => 0,
                Direction.Right => 1,
                Direction.Bottom => 0,
                _ => throw new ArgumentOutOfRangeException()
            };

            y += Direction switch
            {
                Direction.Left => 0,
                Direction.Top => 1,
                Direction.Right => 0,
                Direction.Bottom => -1,
                _ => throw new ArgumentOutOfRangeException()
            };

            return new Point(x, y);
        }
    }
}
