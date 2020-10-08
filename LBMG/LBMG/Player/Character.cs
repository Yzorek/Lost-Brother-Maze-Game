using System;
using System.Collections.Generic;
using System.Text;
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
        public Point Position { get; set; }
        public Character(string name, float speed)
        {
            Name = name;
            Speed = speed;
            Direction = Direction.Bottom;
            IsMoving = false;
            Position = new Point(0, 0);
        }

        public void SpawnAt(int x, int y/*, Map map*/)
        {
            Position = new Point(x, y);
        }

        public void Move()
        {
            int x = Position.X;
            int y = Position.Y;

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

            Position = new Point(x, y);
        }
    }

    public enum Direction
    {
        Left,
        Top,
        Right,
        Bottom
    }
}
