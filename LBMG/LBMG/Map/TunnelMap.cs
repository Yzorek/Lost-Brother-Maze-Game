using LBMG.Tools;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Map
{
    public class TunnelMap
    {
        public TiledMap TiledMap { get; }
        public TiledMapObjectLayer CollisionLayer { get; }
        public TiledMapObjectLayer WalkableLayer { get; }
        public TiledMapLayer BackLayer { get; }
        public TiledMapLayer FrontLayer { get; }

        public TunnelMap(TiledMap tiledMap)
        {
            TiledMap = tiledMap ?? throw new ArgumentNullException(nameof(tiledMap));

            foreach (TiledMapLayer tml in TiledMap.Layers)
            {
                string[] name = tml.Name.ToLower().Split(' ', StringSplitOptions.None);

                if (name.Length == 2
                    && name[0] == "collision"
                    && name[1] == "layer"
                    && tml is TiledMapObjectLayer collisionLayer)
                {
                    CollisionLayer = collisionLayer;
                }
                else if (name.Length == 2
                    && name[0] == "spawn"
                    && name[1] == "layer"
                    && tml is TiledMapObjectLayer spawnLayer)
                {
                    // Deprecated
                }
                else if (name.Length == 2
                   && name[0] == "walkable"
                   && name[1] == "layer"
                   && tml is TiledMapObjectLayer walkableLayer)
                {
                    WalkableLayer = walkableLayer;
                }
                else if (name.Length == 2
                    && name[0] == "layer"
                    && int.TryParse(name[1], out int layerNumber))
                {
                    if (layerNumber == 1)
                        BackLayer = tml;
                    else if (layerNumber == 2)
                        FrontLayer = tml;
                }
            }
        }

        public IEnumerable<Point> GetWalkableCases()
        {
            foreach (TiledMapObject tmObj in WalkableLayer.Objects)
            {
                // TODO Accurate player coords.
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
