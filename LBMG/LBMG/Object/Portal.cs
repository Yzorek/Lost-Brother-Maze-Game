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
    public class Portal : GameObject
    {
        private float _counter;

        public Portal DestinationPortal { get; set; }
        public override float DrawingScale => 1.5f;
        public override Size CaseSize => new Size(3, 3);

        public Portal(string name, ObjectState state, Point coordinates, Portal destinationPortal = null, Character owner = null) : base(name, state, coordinates, owner)
        {
            Rect = new Rectangle(0, 0, 60, 69);
            Sprite = GameObjectSprite.Portal;
            DestinationPortal = destinationPortal;
            _counter = 0;
        }

        public override void OnTriggered(Character fromChar)
        {
            Debug.WriteLine("Walked on " + Name + " by " + fromChar.Name);

            base.OnTriggered(fromChar);

            fromChar.SpawnAt(DestinationPortal.Coordinates.X - 1, DestinationPortal.Coordinates.Y - 1);
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

            Rectangle newRect = Rect;

            int interval = 600;

            if (_counter > 4 * interval)
                _counter = 0;

            if (_counter > 0 && _counter <= interval)
                newRect.Location = new Point(59, 0);
            if (_counter > interval && _counter <= 2 * interval)
                newRect.Location = new Point(119, 0);
            if (_counter > 2 * interval && _counter <= 3 * interval)
                newRect.Location = new Point(178, 0);
            if (_counter > 3 * interval && _counter <= 4 * interval)
                newRect.Location = new Point(5, 0);

            Rect = newRect;
        }
    }
}
