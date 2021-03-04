using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LBMG.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace LBMG.Player
{
    public class Controller
    {
        private List<Direction> _pressedDirs = new List<Direction>();

        public Direction Direction { get; private set; }

        public bool IsKeyPressed { get; private set; }

        public Controller()
        {
            Direction = Direction.Bottom;
            IsKeyPressed = false;
        }

        public void Update(KeyboardStateExtended kse)
        {
            AssignPressedKeys(kse);

            IsKeyPressed = _pressedDirs.Count != 0;

            if (IsKeyPressed)
                Direction = _pressedDirs.Last();
        }

        private void AssignPressedKeys(KeyboardStateExtended kse)
        {
            var keysDirs = new Dictionary<Keys, Direction>
            {
                { Keys.Down, Direction.Bottom },
                { Keys.Up, Direction.Top },
                { Keys.Left, Direction.Left },
                { Keys.Right, Direction.Right }
            };

            foreach (Keys key in keysDirs.Keys)
            {
                if (kse.WasKeyJustUp(key))
                    _pressedDirs.Add(keysDirs[key]);
                else if (kse.WasKeyJustDown(key))
                    _pressedDirs.Remove(keysDirs[key]);
            }
        }
    }
}
