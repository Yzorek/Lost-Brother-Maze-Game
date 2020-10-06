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
        public Character(string name, float speed)
        {
            Name = name;
            Speed = speed;
            Direction = Direction.Bottom;
            IsMoving = false;
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
