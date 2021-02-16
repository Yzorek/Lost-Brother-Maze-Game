using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LBMG.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace LBMG.Map
{
    public class Piece
    {
        public string Name { get; set; }

        public TunnelMap TunnelMap { get; set; }

        public Point Location { get; set; }

        public Piece(TunnelMap tunnelMap, int locX, int locY)
        {
            TunnelMap = tunnelMap;
            Name = tunnelMap.TiledMap.Name;
            Location = new Point(locX, locY);
        }


    }
}
