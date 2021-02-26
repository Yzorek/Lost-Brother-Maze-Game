using LBMG.Player;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Object
{
    public class GameObjectSet
    {
        public List<GameObject> Objects { get; } = new List<GameObject>();

        public GameObjectSet()
        {

        }

        public void Character_Moved(object sender, Point newCoords)
        {
            var senderChar = sender as Character;

            foreach (var obj in Objects)
            {
                if (newCoords.X >= obj.Coordinates.X && newCoords.Y <= obj.Coordinates.Y
                    && newCoords.X < obj.Coordinates.X + obj.CaseSize.Width && newCoords.Y > obj.Coordinates.Y - obj.CaseSize.Height)
                { // When a player trigger it, we notify the object about
                    obj.OnTriggered(senderChar);
                }
            }
        }

        // Is collision ()
        // On Player Clicked

        public void Update(GameTime gameTime, Camera<Vector2> camera)
        {
             // Not doing anything yet
        }
    }
}
