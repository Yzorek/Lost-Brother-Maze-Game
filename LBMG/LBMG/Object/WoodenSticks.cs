using LBMG.Player;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Object
{
    class WoodenSticks : GameObject
    {
        public override Size CaseSize => new Size(2, 2);
        public override ObjectTriggerApproach TriggerApproach => ObjectTriggerApproach.Click;

        public WoodenSticks(string name, ObjectState state, Point coordinates, Character owner = null) : base(name, state, coordinates, owner)
        {
            Rect = new Rectangle(0, 0, 32, 32);
            Sprite = GameObjectSprite.WoodenSticks;
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

        public override void Update(GameTime gameTime, Camera<Vector2> camera)
        {
        }
    }
}
