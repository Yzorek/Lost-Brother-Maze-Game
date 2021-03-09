using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LBMG.Map;
using Microsoft.Xna.Framework;

namespace LBMG.Object
{
    public class PortalSystem
    {
        const int PortalGenFreqOn = 2;

        GameObjectSet _gObjSet;
        Map.Map _map;

        public PortalSystem(GameObjectSet gObjSet, Map.Map map)
        {
            _gObjSet = gObjSet;
            _map = map;
        }

        /// <summary>
        /// Spread connected portals at random locations on the map
        /// </summary>
        /// <param name="count">An even number</param>
        public void SpreadPortalsOnMap(int count)
        {
            if (count > _map.PiecesDictionary.Count)
                throw new ArgumentException("Count too big", nameof(count));
            if (count % 2 != 0)
                throw new ArgumentException("Count has to be an even number", nameof(count));

            var rnd = new Random();
            IEnumerable<Piece> tookPieces = _map.PiecesDictionary.Values
                .OrderBy(x => rnd.Next())
                .Take(count);

            IEnumerable<Point> coordinatesToLayPortals = tookPieces.Select(piece =>
               {
                   var wk = piece.TunnelMap.GetPortalCases();
                   return Map.Map.GetMapCoordsFromPieceCase(piece.Location, wk.ElementAt(rnd.Next(wk.Count())));
               });

            Portal[] layingPortals = coordinatesToLayPortals
                .Select(coord => new Portal("Portal", ObjectState.OnGround, coord))
                .ToArray();

            for (int i = 1; i < layingPortals.Length; i += 2)
            {
                layingPortals[i - 1].DestinationPortal = layingPortals[i];
                layingPortals[i].DestinationPortal = layingPortals[i - 1];
            }

            _gObjSet.Objects.AddRange(layingPortals);
        }

        public void SpreadPortalsOnMap()
        {
            int count = _map.PiecesDictionary.Count / PortalGenFreqOn;
            count -= count % 2;
            SpreadPortalsOnMap(count);
        }
    }
}
