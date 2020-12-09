using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using LBMG.Tools;

namespace LBMG.Map
{
    class MapGenerator
    {
        readonly Random _rnd = new Random();

        List<List<(int, int)>> _paths;
        private Range<int> _pathLengthRg;
        private int _mapSize;
        private double _outletProbPercentage;
        private double _outletMinDistancePercentage;

        public MapGenerator()
        {
        }

        /// <summary>
        /// Returns a new generated map
        /// </summary>
        public GeneratedMap GenerateMap(Range<int> pathLengthRg, int mapSize, double outletProbPercentage, double outletMinDistancePercentage)
        {
            _pathLengthRg = pathLengthRg;
            _mapSize = mapSize;
            _outletProbPercentage = outletProbPercentage;
            _outletMinDistancePercentage = outletMinDistancePercentage;
            _paths = new List<List<(int, int)>>();

            AddPath((0, 0));

            var rMap = ConvertPointMapToMap();
            return rMap;
        }

        private void AddPath((int, int) startingPoint, int index = 0)
        {
            if (index == _mapSize)
                return;

            var (pointX, pointY) = startingPoint;
            var path = new List<(int, int)> { };

            _paths.Add(path);
            // Add first point
            path.Add((pointX, pointY));


            // Get possible directions
            List<Direction> possibleDirections = new List<Direction>();
            if (!_paths.Any(p => p.Contains((pointX - 1, pointY))))
                possibleDirections.Add(Direction.Left);
            if (!_paths.Any(p => p.Contains((pointX + 1, pointY))))
                possibleDirections.Add(Direction.Right);
            if (!_paths.Any(p => p.Contains((pointX, pointY + 1))))
                possibleDirections.Add(Direction.Bottom);
            if (!_paths.Any(p => p.Contains((pointX, pointY - 1))))
                possibleDirections.Add(Direction.Top);

            if (possibleDirections.Count == 0) // Couldn't go farther
                return;

            Direction goingDir = possibleDirections[_rnd.Next(possibleDirections.Count)];
            int pathLength = _rnd.Next(_pathLengthRg);

            for (int i = 0; i < pathLength; i++)
            {
                // Know where to go then
                switch (goingDir)
                {
                    case Direction.Bottom:
                        pointY++;
                        break;
                    case Direction.Top:
                        pointY--;
                        break;
                    case Direction.Right:
                        pointX++;
                        break;
                    case Direction.Left:
                        pointX--;
                        break;
                }

                path.Add((pointX, pointY));
            }

            int waysCount = (_rnd.Next(100) < 100 * _outletProbPercentage) ? 2 : 1;

            for (int i = 0; i < waysCount; i++)
            {
                var newStartingPoint = path[_rnd.Next(1, path.Count)];
                AddPath(newStartingPoint, index + 1);
            }
        }

        GeneratedMap ConvertPointMapToMap()
        {
            GeneratedMap map = new GeneratedMap();

            foreach (var path in _paths)
                for (int i = 0; i < path.Count - 1; i++)
                {
                    (int, int) p1 = path[i],
                        p2 = path[i + 1];
                    var (x1, y1) = p1;
                    var (x2, y2) = p2;
                    int deltaX = x2 - x1,
                        deltaY = y2 - y1;

                    if (deltaX == 1)
                    {
                        map.AssignDirectionAt(x1, y1, Direction.Right);
                        map.AssignDirectionAt(x2, y2, Direction.Left);
                    }
                    else if (deltaX == -1)
                    {
                        map.AssignDirectionAt(x1, y1, Direction.Left);
                        map.AssignDirectionAt(x2, y2, Direction.Right);
                    }
                    if (deltaY == 1)
                    {
                        map.AssignDirectionAt(x1, y1, Direction.Bottom);
                        map.AssignDirectionAt(x2, y2, Direction.Top);
                    }
                    else if (deltaY == -1)
                    {
                        map.AssignDirectionAt(x1, y1, Direction.Top);
                        map.AssignDirectionAt(x2, y2, Direction.Bottom);
                    }
                }

            return map;
        }
    }



    internal class ConsoleGMapDrawer
    {
        private GeneratedMap _map;
        public ConsoleGMapDrawer(GeneratedMap map)
        {
            _map = map;
        }

        public void Draw(bool seeLines)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            for (int i = 0; i < _map.Boundaries.Height * 3 + 3; i++)
            {
                Console.WriteLine(seeLines && i > 0 && i % 3 == 0 ? '\n' + new string('-', _map.Boundaries.Width * 3 + (_map.Boundaries.Width) + 3) : null);

                for (int j = 0; j < _map.Boundaries.Width * 3 + 3; j++)
                {
                    int mi = i / 3, mj = j / 3;

                    var currentPiece = _map.GetDirectionsAt(mj +_map.Boundaries.Left, mi + _map.Boundaries.Top);

                    if (seeLines && j > 0 && j % 3 == 0)
                        Console.Write('|');

                    switch ((j % 3, i % 3))
                    {
                        case (0, 1) when currentPiece.Contains(Direction.Left):
                        case (1, 0) when currentPiece.Contains(Direction.Top):
                        case (2, 1) when currentPiece.Contains(Direction.Right):
                        case (1, 2) when currentPiece.Contains(Direction.Bottom):
                        case (1, 1) when currentPiece.Contains(Direction.Bottom) || currentPiece.Contains(Direction.Right) || currentPiece.Contains(Direction.Left) || currentPiece.Contains(Direction.Top):
                            Console.Write((char)9608);
                            break;
                        default:
                            Console.Write(' ');
                            break;
                    }
                    //Console.Write('');
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

// TODO Replace (int, int) with XNA Point 