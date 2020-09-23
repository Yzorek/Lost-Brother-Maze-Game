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
        public Character(string name)
        {
            Name = name;
        }
    }
}
