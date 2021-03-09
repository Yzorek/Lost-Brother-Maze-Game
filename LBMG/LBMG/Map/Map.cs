﻿using LBMG.Object;
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
        public Dictionary<Point, Piece> PiecesDictionary { get; }
        public Difficulty Difficulty { get; }
        public Point[] SpawnCoordinates { get; private set; }

        protected Map(Difficulty difficulty)
        {
            Difficulty = difficulty;
            PiecesDictionary = new Dictionary<Point, Piece>();
        }

        public bool IsCollision(Point coordinates, GameObjectSet goSet)
        {
            if (goSet.IsCollision(coordinates))
                return true;

            Point fixedCoordinates = coordinates;
            fixedCoordinates.Y *= -1;

            Vector2 pos = fixedCoordinates.ToVector2() / Constants.TiledMapSize;

            Point tiledMapLocation = new Point((int)Math.Round(pos.X, MidpointRounding.ToNegativeInfinity), (int)Math.Round(pos.Y, MidpointRounding.ToNegativeInfinity));

            if (!PiecesDictionary.ContainsKey(tiledMapLocation))
                return false;

            Point onPiecePos = fixedCoordinates - new Point(Constants.TiledMapSize * tiledMapLocation.X, Constants.TiledMapSize * tiledMapLocation.Y);

            
            return PiecesDictionary[tiledMapLocation].TunnelMap.IsCollision(onPiecePos);
        }

        public  IEnumerable<Point> GetCases(Func<TunnelMap, IEnumerable<Point>> getCasesFunc) => PiecesDictionary.Values
                .SelectMany(x => getCasesFunc(x.TunnelMap)
                    .Select(y => GetMapCoordsFromPieceCase(x.Location, y)));

        public static Map Create(Difficulty difficulty, TunnelMapFactory tmFactory)
        {
            Map map = new Map(difficulty);

            Random random = new Random();

            // Generating Map
            MapGenerator mg = new MapGenerator();
            GeneratedMap generatedMap = mg.GenerateMap(new Range<int>(2), 7, 1, 0);//(new Range<int>(2), 50, 1, 0);
            var generatedSpawnPositions = generatedMap.GetNewSpawnLocations(2, 4);

            List<Point> spawnCoordsList = new List<Point>();

#if DEBUG
            ConsoleGMapDrawer cgmd = new ConsoleGMapDrawer(generatedMap);
            cgmd.Draw(true);
#endif

            var directionsTMEquivalent = new Dictionary<HashSet<Direction>, Tunnel>
               {
                    { new HashSet<Direction> { Direction.Left, Direction.Top, Direction.Right, Direction.Bottom }, Tunnel.CrossRoad },
                    { new HashSet<Direction> { Direction.Bottom, Direction.Left }, Tunnel.BottomLeft },
                    { new HashSet<Direction> { Direction.Bottom, Direction.Right }, Tunnel.BottomRight  },
                    { new HashSet<Direction> { Direction.Top, Direction.Right }, Tunnel.TopRight  },
                    { new HashSet<Direction> { Direction.Bottom }, Tunnel.Bottom  },
                    { new HashSet<Direction> { Direction.Top, Direction.Bottom, Direction.Left }, Tunnel.VerticalLeft  },
                    { new HashSet<Direction> { Direction.Left, Direction.Top }, Tunnel.TopLeft  },
                    { new HashSet<Direction> { Direction.Top }, Tunnel.Top  },
                    { new HashSet<Direction> { Direction.Bottom, Direction.Top }, Tunnel.Vertical  },
                    { new HashSet<Direction> { Direction.Left, Direction.Right }, Tunnel.Horizontal  },
                    { new HashSet<Direction> { Direction.Left, Direction.Right, Direction.Bottom }, Tunnel.HorizontalBottom  },
                    { new HashSet<Direction> { Direction.Left, Direction.Right, Direction.Top }, Tunnel.HorizontalTop  },
                    { new HashSet<Direction> { Direction.Bottom, Direction.Top, Direction.Right }, Tunnel.VerticalRight },
                    { new HashSet<Direction> { Direction.Right }, Tunnel.Right  },
                    { new HashSet<Direction> { Direction.Left }, Tunnel.Left  },
                };


#if DEBUG  // For clean tests sometimes
            bool __nomap = false,
                __onlycrossroad = true,
                __onetile = false;
#endif

            foreach (var dirsPiece in generatedMap.GetPieces())
            {

                Point tiledMapLocation = new Point(dirsPiece.Item1.Item1, dirsPiece.Item1.Item2);

#if DEBUG
                if (__onetile)
                {
                    tiledMapLocation = new Point(0, 0);
                    if (map.PiecesDictionary.ContainsKey(tiledMapLocation))
                        break;
                }
#endif

                HashSet<Direction> directions = dirsPiece.Item2;

                var dtmeDictKey = directionsTMEquivalent.Keys
                    .Where(x => directions.SetEquals(x))
                    .FirstOrDefault();

                Piece piece;
#if DEBUG
                if (__onlycrossroad)
                    piece = new Piece(tmFactory.GetTunnelMap(Tunnel.CrossRoad), tiledMapLocation.X, tiledMapLocation.Y);
                else
#endif
                    piece = new Piece(tmFactory.GetTunnelMap(directionsTMEquivalent[dtmeDictKey]), tiledMapLocation.X, tiledMapLocation.Y);

#if DEBUG
                if (!__nomap)
#endif
                    map.PiecesDictionary.Add(tiledMapLocation, piece);

                // Add new spawn coords when we're on it
                if (generatedSpawnPositions.Contains(dirsPiece.Item1))
                {
                    Point[] allPieceSpawnPointCoordinates = piece.TunnelMap.GetWalkableCases().ToArray();
                    Point pieceSpawnPointCoordinates = allPieceSpawnPointCoordinates[random.Next(allPieceSpawnPointCoordinates.Length)];
                    Point coords = GetMapCoordsFromPieceCase(tiledMapLocation, pieceSpawnPointCoordinates);
                    spawnCoordsList.Add(coords);
                }
            }

            map.SpawnCoordinates = spawnCoordsList.ToArray();

            return map;
        }

        public static Point GetMapCoordsFromPieceCase(Point pieceLoc, Point pieceCase)
        {
            Point coords = new Point(pieceLoc.X * Constants.TiledMapSize, pieceLoc.Y * Constants.TiledMapSize) + pieceCase;
            coords.Y *= -1;
            return coords;
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
