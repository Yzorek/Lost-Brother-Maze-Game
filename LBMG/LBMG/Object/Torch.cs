using LBMG.Player;
using LBMG.Tools;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LBMG.Object
{
    public class Torch : GameObject
    {
        private float _counter;

        public bool IsBurning { get; set; }

        public override float DrawingScale => 0.75f;
        public override Size CaseSize => new Size(1, 2);
        public override ObjectTriggerApproach TriggerApproach => ObjectTriggerApproach.Walk;

        public Torch(string name, ObjectState state, Point coordinates, Character owner = null) : base(name, state, coordinates, owner)
        {
            Rect = Constants.TorchRect();
            Sprite = GameObjectSprite.TorchLightened;
            IsBurning = false;
            _counter = 0;
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
            _counter += 2 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_counter > 1800)
                _counter = 0;
            
            Rectangle newRect = Rect;
            if (_counter > 0 && _counter <= 200)
                newRect.Location = new Point(37, 0);
            if (_counter > 200 && _counter <= 400)
                newRect.Location = new Point(77, 0);
            if (_counter > 400 && _counter <= 600)
                newRect.Location = new Point(113, 0);
            if (_counter > 600 && _counter <= 800)
                newRect.Location = new Point(145, 0);
            if (_counter > 800 && _counter <= 1000)
                newRect.Location = new Point(177, 0);
            if (_counter > 1000 && _counter <= 1200)
                newRect.Location = new Point(213, 0);
            if (_counter > 1200 && _counter <= 1400)
                newRect.Location = new Point(247, 0);
            if (_counter > 1400 && _counter <= 1600)
                newRect.Location = new Point(283, 0);
            if (_counter > 1600 && _counter <= 1800)
                newRect.Location = new Point(3, 0);
            Rect = newRect;
        }
    }
}
