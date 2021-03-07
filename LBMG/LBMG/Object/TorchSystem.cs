using LBMG.Object;
using LBMG.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
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
        }

        void AddTorch(Point coordinates)
        {
            Torch newTorch = new Torch("Torch", ObjectState.OnGround, coordinates);
            _gameObjectSet.Objects.Add(newTorch);
        }
    }
}
