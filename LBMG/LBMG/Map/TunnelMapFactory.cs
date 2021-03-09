using LBMG.Tools;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LBMG.Map
{
    public enum Tunnel
    {
        CrossRoad, BottomLeft, BottomRight, Bottom, HorizontalBottom, 
        HorizontalTop, Horizontal, Left, Right, TopLeft, 
        TopRight, Top, VerticalLeft, VerticalRight, Vertical
    }

    public class TunnelMapFactory
    {
        private Dictionary<Tunnel, TunnelMap> _tunnelMaps;

        public IEnumerable<TunnelMap> LoadedMaps => _tunnelMaps.Values;

        public void LoadTunnelMaps(ContentManager cm)
        {
            _tunnelMaps = new Dictionary<Tunnel, TunnelMap>
            {
                { Tunnel.CrossRoad, new TunnelMap(cm.Load<TiledMap>("TiledMaps/cross_road_tunnel")) },
                //{ Tunnel.BottomLeft, new TunnelMap(cm.Load<TiledMap>("TiledMaps/bottom_left_tunnel")) },
                //{ Tunnel.BottomRight, new TunnelMap(cm.Load<TiledMap>("TiledMaps/bottom_right_tunnel")) },
                //{ Tunnel.Bottom, new TunnelMap(cm.Load<TiledMap>("TiledMaps/bottom_tunnel")) },
                //{ Tunnel.HorizontalBottom, new TunnelMap(cm.Load<TiledMap>("TiledMaps/horizontal_bottom_tunnel")) },
                //{ Tunnel.HorizontalTop, new TunnelMap(cm.Load<TiledMap>("TiledMaps/horizontal_top_tunnel")) },
                //{ Tunnel.Horizontal, new TunnelMap(cm.Load<TiledMap>("TiledMaps/horizontal_tunnel")) },
                //{ Tunnel.Left, new TunnelMap(cm.Load<TiledMap>("TiledMaps/left_tunnel")) },
                //{ Tunnel.Right, new TunnelMap(cm.Load<TiledMap>("TiledMaps/right_tunnel")) },
                //{ Tunnel.TopLeft, new TunnelMap(cm.Load<TiledMap>("TiledMaps/top_left_tunnel")) },
                //{ Tunnel.TopRight, new TunnelMap(cm.Load<TiledMap>("TiledMaps/top_right_tunnel")) },
                //{ Tunnel.Top, new TunnelMap(cm.Load<TiledMap>("TiledMaps/top_tunnel")) },
                //{ Tunnel.VerticalLeft, new TunnelMap(cm.Load<TiledMap>("TiledMaps/vertical_left_tunnel")) },
                //{ Tunnel.VerticalRight, new TunnelMap(cm.Load<TiledMap>("TiledMaps/vertical_right_tunnel")) },
                //{ Tunnel.Vertical, new TunnelMap(cm.Load<TiledMap>("TiledMaps/vertical_tunnel")) }
            };
        }

        public TunnelMap GetTunnelMap(Tunnel tunnel)
        {
            return _tunnelMaps[tunnel];
        }
    }
}
