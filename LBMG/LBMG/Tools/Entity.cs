using LBMG.Object;
using LBMG.Player;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Tools
{
    public static class Entity
    {
        public static Vector2 GetEntityDrawingPosByCamera(Character character, Camera<Vector2> camera, Vector2 centerPos)
        {
            return camera.WorldToScreen(GetPixelPosFromCoordinates(character.Coordinates)) + centerPos;
        }

        public static Vector2 GetEntityDrawingPosByCamera(GameObject obj, Camera<Vector2> camera, Vector2 centerPos)
        {
            return camera.WorldToScreen(GetPixelPosFromCoordinates(obj.Coordinates)) + centerPos;
        }

        static Vector2 GetPixelPosFromCoordinates(Point coordinates)
        {
            return new Vector2(coordinates.X * (Constants.TileSize), -coordinates.Y * (Constants.TileSize));
        }
    }
}
