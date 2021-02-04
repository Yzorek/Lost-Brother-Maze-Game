using LBMG.Player;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Object
{
    public class Torch : GameObject
    {
        public bool IsBurning { get; set; }

        public Torch(string name, ObjectState state, Point coordinates, Character owner = null) : base(name, state, coordinates, owner)
        {
            IsBurning = false;
        }

        public override void Take(Character character)
        {
            base.Take(character);
        }

        public override void PutInInventory()
        {
            base.PutInInventory();
        }

        public override void Drop()
        {
            base.Drop();
        }

        public override void Update(GameTime gameTime, Camera<Vector2> camera, Rectangle rect)
        {
            base.Update(gameTime, camera, rect);
        }
    }
}
