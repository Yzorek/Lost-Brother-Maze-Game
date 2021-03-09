using LBMG.Tools;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBMG.Map
{
    public class TunnelMap
    {
        public TiledMap TiledMap { get; }
        public TiledMapObjectLayer CollisionLayer { get; }
        public TiledMapObjectLayer WalkableLayer { get; }
        public TiledMapObjectLayer PortalLayer { get; }
        public TiledMapObjectLayer WoodenSticksLayer { get; }
        public TiledMapLayer BackLayer { get; }
        public TiledMapLayer FrontLayer { get; }

        public TunnelMap(TiledMap tiledMap)
        {
            TiledMap = tiledMap ?? throw new ArgumentNullException(nameof(tiledMap));

            // Fill properties from found layers
            foreach (TiledMapLayer tml in TiledMap.Layers)
            {
                string[] name = tml.Name.ToLower().Split(' ', StringSplitOptions.None);

                if (name.Length == 2
                    && name[0] == "layer"
                    && int.TryParse(name[1], out int layerNumber))
                {
                    if (layerNumber == 1)
                        BackLayer = tml;
                    else if (layerNumber == 2)
                        FrontLayer = tml;
                }
                else if (name[0] == "ops" && tml is TiledMapGroupLayer tmgl)
                {
                    foreach (TiledMapObjectLayer tmol in tmgl.Layers)
                    {
                        string[] opName = tmol.Name.ToLower().Split(' ', StringSplitOptions.None);
                        switch (opName[0])
                        {
                            case "walkable":
                                WalkableLayer = tmol;
                                break;
                            case "collision":
                                CollisionLayer = tmol;
                                break;
                            case "spawn":
                                break; // Deprecated
                            case "portal":
                                PortalLayer = tmol;
                                break;
                            case "ws":
                                WoodenSticksLayer = tmol;
                                break;
                        }
                    }
                }
            }
        }

        public IEnumerable<Point> GetWalkableCases()
        {
            foreach (TiledMapObject tmObj in WalkableLayer.Objects)
            {
                // TODO Accurate player coords. EDIT ???
                Point coords = (tmObj.Position / Constants.TileSize).ToPoint();
                Point oppCorner = ((tmObj.Position + (Vector2)tmObj.Size) / Constants.TileSize).ToPoint();

                for (int x = coords.X; x < oppCorner.X; x++)
                    for (int y = coords.Y; y < oppCorner.Y; y++)
                    {
                        Point p = new Point(x, y);
                        if (!IsCollision(p))
                            yield return p;
                    }

            }
            //return _walkableLayer.Objects.Select(tmObj => (tmObj.Position / Constants.TileSize).ToPoint());
        }

        public IEnumerable<Point> GetPortalCases()
        {
            return PortalLayer.Objects.Select(tmObj => (tmObj.Position / Constants.TileSize).ToPoint());
        }

        public IEnumerable<Point> GetWoodenSticksCases()
        {
            return WoodenSticksLayer.Objects.Select(tmObj => (tmObj.Position / Constants.TileSize).ToPoint());
        }

        public bool IsCollision(Point onPieceCoordinates)
        {
            return IsGenuineCollision(onPieceCoordinates)
                || IsGenuineCollision(onPieceCoordinates + new Point(1, 0)); // Check is not too much on the right actually
        }


        private bool IsGenuineCollision(Point onPieceCoordinates)
        {
            Point pos = new Point(onPieceCoordinates.X * Constants.TileSize + Constants.TileSize / 2, onPieceCoordinates.Y * Constants.TileSize + Constants.TileSize / 2);

            foreach (TiledMapObject tmObj in CollisionLayer.Objects)
            {
                if (pos.X >= tmObj.Position.X && pos.Y >= tmObj.Position.Y
                       && pos.X <= tmObj.Position.X + tmObj.Size.Width && pos.Y <= tmObj.Position.Y + tmObj.Size.Height)
                    return true;
            }

            return false;
        }
    }
}
