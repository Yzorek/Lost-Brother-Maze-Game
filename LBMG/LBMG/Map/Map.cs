using LBMG.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            TiledMapsDictionary = new Dictionary<Point, Piece>();
        }

        public void LoadMap(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            // Generating Map
            MapGenerator mg = new MapGenerator();
            GeneratedMap generatedMap = mg.GenerateMap(new Range<int>(2), 7, 1, 0);//(new Range<int>(2), 50, 1, 0);

            //
            //ConsoleGMapDrawer cgmd = new ConsoleGMapDrawer(generatedMap);
            //cgmd.Draw(true);
            //TiledMap crossRoad = cm.Load<TiledMap>("TiledMaps/cross_road_tunnel");

            var directionsTMEquivalent = new Dictionary<HashSet<Direction>, TiledMap>
               {
                    { new HashSet<Direction> { Direction.Left, Direction.Top, Direction.Right, Direction.Bottom }, cm.Load<TiledMap>("TiledMaps/cross_road_tunnel") },
                    { new HashSet<Direction> { Direction.Bottom, Direction.Left }, cm.Load<TiledMap>("TiledMaps/bottom_left_tunnel") },
                    { new HashSet<Direction> { Direction.Bottom, Direction.Right }, cm.Load<TiledMap>("TiledMaps/bottom_right_tunnel") },
                    { new HashSet<Direction> { Direction.Top, Direction.Right }, cm.Load<TiledMap>("TiledMaps/top_right_tunnel") },
                    { new HashSet<Direction> { Direction.Bottom }, cm.Load<TiledMap>("TiledMaps/bottom_tunnel") },
                    { new HashSet<Direction> { Direction.Top, Direction.Bottom, Direction.Left }, cm.Load<TiledMap>("TiledMaps/vertical_left_tunnel") },
                    { new HashSet<Direction> { Direction.Left, Direction.Top }, cm.Load<TiledMap>("TiledMaps/top_left_tunnel") },
                    { new HashSet<Direction> { Direction.Top }, cm.Load<TiledMap>("TiledMaps/top_tunnel") },
                    { new HashSet<Direction> { Direction.Bottom, Direction.Top }, cm.Load<TiledMap>("TiledMaps/vertical_tunnel") },
                    { new HashSet<Direction> { Direction.Left, Direction.Right }, cm.Load<TiledMap>("TiledMaps/horizontal_tunnel") },
                    { new HashSet<Direction> { Direction.Left, Direction.Right, Direction.Bottom }, cm.Load<TiledMap>("TiledMaps/horizontal_bottom_tunnel") },
                    { new HashSet<Direction> { Direction.Left, Direction.Right, Direction.Top }, cm.Load<TiledMap>("TiledMaps/horizontal_top_tunnel") },
                    { new HashSet<Direction> { Direction.Bottom, Direction.Top, Direction.Right }, cm.Load<TiledMap>("TiledMaps/vertical_right_tunnel") },
                    { new HashSet<Direction> { Direction.Right }, cm.Load<TiledMap>("TiledMaps/right_tunnel") },
                    { new HashSet<Direction> { Direction.Left }, cm.Load<TiledMap>("TiledMaps/left_tunnel") },
                };

            

            foreach (var dirsPiece in generatedMap.GetPieces())
            {
                Point tiledMapLocation = new Point(dirsPiece.Item1.Item1, dirsPiece.Item1.Item2);
                //tiledMapLocation = new Point(0, 0);
                //if (TiledMapsDictionary.ContainsKey(tiledMapLocation))
                //    break;

                HashSet<Direction> directions = dirsPiece.Item2;

                var dtmeDictKey = directionsTMEquivalent.Keys
                    .Where(x => directions.SetEquals(x))
                    .FirstOrDefault();

                //if (dtmeDictKey != null)

                // var piece = new Piece(crossRoad, tiledMapLocation.X, tiledMapLocation.Y);
                var piece = new Piece(directionsTMEquivalent[dtmeDictKey], tiledMapLocation.X, tiledMapLocation.Y);
                piece.Initialize(gd, window);
                TiledMapsDictionary.Add(tiledMapLocation, piece);
            }
        }

        public bool IsCollision(Point coordinates)
        {
            Point fixedCoordinates = coordinates;
            fixedCoordinates.Y *= -1;

            Vector2 pos = fixedCoordinates.ToVector2() / Constants.TiledMapSize;

            Point tiledMapLocation = new Point((int)Math.Round(pos.X, MidpointRounding.ToNegativeInfinity), (int)Math.Round(pos.Y, MidpointRounding.ToNegativeInfinity));

            if (!TiledMapsDictionary.ContainsKey(tiledMapLocation))
                return false;

            Point onPiecePos = fixedCoordinates - new Point(Constants.TiledMapSize * tiledMapLocation.X, Constants.TiledMapSize * tiledMapLocation.Y);

            bool r = TiledMapsDictionary[tiledMapLocation].IsCollision(onPiecePos);
            return r;
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
