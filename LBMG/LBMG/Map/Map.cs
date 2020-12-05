using LBMG.Tools;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LBMG.Map
{
    public class Map
    {
        public Dictionary<Point, Piece> TiledMapsDictionary { get; set; }

        public Difficulty Difficulty { get; set; }

        public Map(Difficulty difficulty)
        {
            TiledMapsDictionary = null;
            Difficulty = difficulty;
        }

        public void LoadMap(GraphicsDevice gd, ContentManager cm)
        {
            TiledMapsDictionary = new Dictionary<Point, Piece>
            {
                {new Point(0, 0), new Piece(gd, cm.Load<TiledMap>("TiledMaps/bottom_right_tunnel"), 0, 0)},
                {new Point(1, 0), new Piece(gd, cm.Load<TiledMap>("TiledMaps/bottom_left_tunnel"), 512 + (int) (512 * Constants.ZoomFact), 0)},
                {new Point(0, 1), new Piece(gd, cm.Load<TiledMap>("TiledMaps/top_right_tunnel"), 0, 512 + (int) (512 * Constants.ZoomFact))},
                {new Point(1, 1), new Piece(gd, cm.Load<TiledMap>("TiledMaps/cross_road"), 512 + (int) (512 * Constants.ZoomFact), 512 + (int) (512 * Constants.ZoomFact))}
            };
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
