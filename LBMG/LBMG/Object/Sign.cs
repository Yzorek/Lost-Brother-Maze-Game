using LBMG.Player;
using LBMG.Tools;
using LBMG.UI;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LBMG.Object
{
    public class Sign : GameObject
    {
        DialogBox _dialogBox;

        public override float DrawingScale => 1;
        public override Size CaseSize => new Size(2, 3);
        public override Point RectangleOffset => new Point(0, 32);
        public override ObjectTriggerApproach TriggerApproach => ObjectTriggerApproach.Click;

        public Sign(string name, ObjectState state, Point coordinates, DialogBox dialogBox, Character owner = null) : base(name, state, coordinates, owner)
        {
            _dialogBox = dialogBox;
            Rect = new Rectangle(0, 0, 32, 64);
            Sprite = GameObjectSprite.Sign;
        }

        public override void OnTriggered(Character fromChar)
        {
            _dialogBox.Write(7, new[] { Coordinates.X.ToString(), Coordinates.Y.ToString() });

            base.OnTriggered(fromChar);
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

        public override void UpdateRectangle(GameTime gameTime, Camera<Vector2> camera)
        {

        }
    }
}
