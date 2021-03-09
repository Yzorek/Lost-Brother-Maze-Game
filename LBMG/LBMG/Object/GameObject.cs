using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using LBMG.Player;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using LBMG.Tools;

namespace LBMG.Object
{
    public abstract class GameObject
    {
        public string Name { get; set; }
        public Point Coordinates { get; set; }
        public ObjectState State { get; set; }
        public Character Owner { get; set; }
        public Rectangle Rect { get; protected set; }
        public GameObjectSprite Sprite { get; protected set; }

        public virtual float DrawingScale { get => 1f; }
        public virtual Point RectangleOffset { get => Point.Zero; }
        public abstract Size CaseSize { get; }
        public abstract ObjectTriggerApproach TriggerApproach { get; }

        public GameObject(string name, ObjectState state, Point coordinates, Character owner = null)
        {
            Name = name;
            State = state;

            if (state == ObjectState.OnGround)
            {
                Owner = null;
                Coordinates = new Point(coordinates.X, coordinates.Y);
            }
            else
            {
                Owner = owner;
                Coordinates = new Point(0, 0);
            }
        }

        /// <summary>
        /// Currently, called when the character walk on
        /// </summary>
        public virtual void OnTriggered(Character fromChar) { }

        public virtual void Take(Character character)
        {
            if (State != ObjectState.OnGround) return;

            State = ObjectState.InHand;
            Owner = character;
        }

        public virtual void PutInInventory()
        {
            if (State != ObjectState.InHand) return;

            State = ObjectState.InInventory;
        }

        public virtual void Drop()
        {
            if (State == ObjectState.OnGround) return;

            State = ObjectState.OnGround;
            Owner = null;
        }

        // DOLATER Here a dictionary, Prefer a handled in drawer system than this 
        public virtual void Update(GameTime gameTime, Camera<Vector2> camera) { }
    }


    public enum ObjectState
    {
        OnGround,
        InHand,
        InInventory
    }

    public enum ObjectTriggerApproach
    {
        Walk, Click
    }
}
