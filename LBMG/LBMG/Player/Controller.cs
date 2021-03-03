using System;
using System.Collections.Generic;
using System.Text;
using LBMG.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace LBMG.Player
{
    public class Controller
    {
        public Direction Direction { get; private set; }

        public bool IsKeyPressed { get; private set; }

        public Controller()
        {
            Direction = Direction.Bottom;
            IsKeyPressed = false;
        }

        public void Update(KeyboardStateExtended kse)
        {
            if (kse.IsKeyDown(Keys.Left))
                Direction = Direction.Left;
            else if (kse.IsKeyDown(Keys.Up))
                Direction = Direction.Top;
            else if (kse.IsKeyDown(Keys.Right))
                Direction = Direction.Right;
            else if (kse.IsKeyDown(Keys.Down))
                Direction = Direction.Bottom;
            else if (kse.IsKeyDown(Keys.Q))
                Direction = Direction.Left;
            else if (kse.IsKeyDown(Keys.Z))
                Direction = Direction.Top;
            else if (kse.IsKeyDown(Keys.D))
                Direction = Direction.Right;
            else if (kse.IsKeyDown(Keys.S))
                Direction = Direction.Bottom;
            else
            {
                IsKeyPressed = false;
                return;
            }
            IsKeyPressed = true;
        }
    }
}
