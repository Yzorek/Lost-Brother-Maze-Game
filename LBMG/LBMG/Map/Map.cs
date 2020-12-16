using LBMG.Tools;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LBMG.Map
{
    public class Map
    {
        private const int TiledMapSize = 512;

        public Dictionary<Point, Piece> TiledMapsDictionary { get; set; }

        public Difficulty Difficulty { get; set; }

        public Map(Difficulty difficulty)
        {
            TiledMapsDictionary = null;
            Difficulty = difficulty;
            TiledMapsDictionary = new Dictionary<Point, Piece>();
        }

        public void LoadMap(GraphicsDevice gd, ContentManager cm)
        {
            // Generating Map
            MapGenerator mg = new MapGenerator();
            GeneratedMap generatedMap = mg.GenerateMap(new Range<int>(2), 7, 1, 0);//(new Range<int>(2), 50, 1, 0);

            //
            //ConsoleGMapDrawer cgmd = new ConsoleGMapDrawer(generatedMap);
            //cgmd.Draw(true);
            TiledMap crossRoad = cm.Load<TiledMap>("TiledMaps/cross_road");

            Dictionary<Direction[], TiledMap> directionsTMEquivalent = new Dictionary<Direction[], TiledMap>
               {
                    { new[] { Direction.Left, Direction.Top, Direction.Right, Direction.Bottom }, cm.Load<TiledMap>("TiledMaps/cross_road") },
                    { new[] { Direction.Bottom, Direction.Left }, cm.Load<TiledMap>("TiledMaps/bottom_left_tunnel") },
                    { new[] { Direction.Bottom, Direction.Right }, cm.Load<TiledMap>("TiledMaps/bottom_right_tunnel") },
                    { new[] { Direction.Top, Direction.Right }, cm.Load<TiledMap>("TiledMaps/top_right_tunnel") },
                    { new[] { Direction.Bottom }, cm.Load<TiledMap>("TiledMaps/bottom_tunnel") },
                    { new[] { Direction.Top, Direction.Bottom, Direction.Left }, cm.Load<TiledMap>("TiledMaps/top_bottom_left_tunnel") },
                    { new[] { Direction.Left, Direction.Top }, cm.Load<TiledMap>("TiledMaps/top_left_tunnel") },
                    { new[] { Direction.Top }, cm.Load<TiledMap>("TiledMaps/top_tunnel") },
                    { new[] { Direction.Bottom, Direction.Top }, cm.Load<TiledMap>("TiledMaps/vertical_tunnel") },
                    { new[] { Direction.Left, Direction.Right }, cm.Load<TiledMap>("TiledMaps/horizontal_tunnel") },
                };

            foreach (var dirsPiece in generatedMap.GetPieces())
            {
                Point position = new Point(dirsPiece.Item1.Item1, dirsPiece.Item1.Item2);
                HashSet<Direction> directions = dirsPiece.Item2;

                int gpPosX = (TiledMapSize + (int)(TiledMapSize * Constants.ZoomFact)) * position.X,
                    gpPosY = (TiledMapSize + (int)(TiledMapSize * Constants.ZoomFact)) * position.Y;

                var dtmeDictKey = directionsTMEquivalent.Keys
                    .Where(x => directions.SetEquals(new HashSet<Direction>(x)))
                    .FirstOrDefault();

                if (dtmeDictKey != null)
                {
                    var piece = new Piece(directionsTMEquivalent[dtmeDictKey], gpPosX, gpPosY);
                    piece.Initialize(gd);
                    TiledMapsDictionary.Add(position, piece);
                }
            }
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
