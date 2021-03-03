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
            GameObject gameObject = GetGameObjectCoveringCoordinates(newCoords);

            if (gameObject != null && gameObject.TriggerApproach == ObjectTriggerApproach.Walk)
            {
                gameObject.OnTriggered(senderChar);
            }
        }

        public void Character_Clicked(object sender, Point towardCoords)
        {
            var senderChar = sender as Character;
            GameObject gameObject = GetGameObjectCoveringCoordinates(towardCoords);

            if (gameObject != null && gameObject.TriggerApproach == ObjectTriggerApproach.Click)
            {
                gameObject.OnTriggered(senderChar);
            }
        }

        public bool IsCollision(Point coordinates)
        {
            GameObject collidingGameObj = GetGameObjectCoveringCoordinates(coordinates);

            return collidingGameObj != null && collidingGameObj.TriggerApproach == ObjectTriggerApproach.Click;
        }

        private GameObject GetGameObjectCoveringCoordinates(Point coordinates)
        {
            foreach (var obj in Objects)
            {
                if (coordinates.X >= obj.Coordinates.X && coordinates.Y <= obj.Coordinates.Y
                    && coordinates.X < obj.Coordinates.X + obj.CaseSize.Width && coordinates.Y > obj.Coordinates.Y - obj.CaseSize.Height)
                { // When a player trigger it, we notify the object about
                    return obj;
                }
            }

            return null;
        }


        public void Update(GameTime gameTime, Camera<Vector2> camera)
        {
             // Not doing anything yet
        }
    }
}
