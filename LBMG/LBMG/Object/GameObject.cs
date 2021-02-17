using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using LBMG.Player;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;

namespace LBMG.Object
{
    public class GameObject
    {
        public string Name { get; set; }
        public Point Coordinates { get; set; }
        public ObjectState State { get; set; }
        public Character Owner { get; set; }

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

        public virtual void Update(GameTime gameTime, Camera<Vector2> camera, ref Rectangle rect)
        {

        }
    }

    public enum ObjectState
    {
        OnGround,
        InHand,
        InInventory
    }
}
