using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace LBMG.Map
{
    public class Piece
    {
        public string Name { get; set; }

        public TiledMap TiledMap { get; set; }

        public Point Position { get; set; }

        public Piece(TiledMap tiledMap, int posX, int posY)
        {
            TiledMap = tiledMap;
            Name = TiledMap.Name;
            Position = new Point(posX, posY);
        }
    }
}
