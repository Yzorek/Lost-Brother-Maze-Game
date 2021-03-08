using LBMG.Object;
using LBMG.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Object
{
    class TorchSystem
    {
        private GameObjectSet _gameObjectSet;

        public TorchSystem(GameObjectSet gameObjectSet)
        {
            _gameObjectSet = gameObjectSet;
        }

        public void Update(GameTime gameTime, KeyboardStateExtended kse, Character activeChar)
        {
            if (kse.WasKeyJustUp(Keys.Space))
            {
                AddTorch(activeChar.GetBehindCoordinates());
            }

            // Remove not burning torches
            var torches = _gameObjectSet.Objects.OfType<Torch>().ToArray();
            for (int i = 0; i < torches.Length; i++)
            {
                Torch torch = torches[i];
                if (!torches[i].IsBurning)
                    _gameObjectSet.Objects.Remove(torch);
            }
        }

        void AddTorch(Point coordinates)
        {
            var newTorch = new Torch("Torch", ObjectState.OnGround, coordinates);
            newTorch.IsBurning = true;
            _gameObjectSet.Objects.Add(newTorch);
        }
    }
}
