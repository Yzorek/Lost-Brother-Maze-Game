using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LBMG.Tools;

namespace LBMG.Map
{
    class GeneratedMap
    {
        private readonly Dictionary<(int, int), HashSet<Direction>> _pieces;

        public Rectangle Boundaries
        {
            get
            {
                int x = _pieces.Keys.Min(p => p.Item1),
                    y = _pieces.Keys.Min(p => p.Item2);
                int width = _pieces.Keys.Max(p => p.Item1) - x,
                    height = _pieces.Keys.Max(p => p.Item2) - y;
                width = width > 0 ? width : 1;
                height = height > 0 ? height : 1;

                return new Rectangle(x, y, width, height);
            }
        }

        public IEnumerable<(int, int)> GetNewSpawnLocations(int count, int minGap)
        {
            Random rnd = new Random();
            List<(int, int)> spawnPositions = new List<(int, int)>();

            var piecesKeys = new (int, int)[_pieces.Count];
            _pieces.Keys.CopyTo(piecesKeys, 0);

            for (int i = 0; i < count; i++)
            {
                int x, y;

                do
                {
                    (x, y) = piecesKeys[rnd.Next(piecesKeys.Length)];
                }
                while (!spawnPositions.All((sp) => Math.Abs(sp.Item1 - x) > minGap && Math.Abs(sp.Item2 - y) > minGap));

                spawnPositions.Add((x, y));
            }

            return spawnPositions;
        }

        public IEnumerable<Tuple<(int, int), HashSet<Direction>>> GetPieces()
        {
            foreach (var pos in _pieces.Keys)
                yield return new Tuple<(int, int), HashSet<Direction>>(pos, _pieces[pos]);
        }

        public IEnumerable<Direction> GetDirectionsAt(int x, int y)
        {
            return _pieces.ContainsKey((x, y)) ? _pieces[(x, y)] : Enumerable.Empty<Direction>();
        }

        public void AssignDirectionAt(int x, int y, Direction direction)
        {
            if (!_pieces.ContainsKey((x, y)))
                _pieces.Add((x, y), new HashSet<Direction>());
            _pieces[(x, y)].Add(direction);
        }

        public GeneratedMap()
        {
            _pieces = new Dictionary<(int, int), HashSet<Direction>>();
        }
    }
}
