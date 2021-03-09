using LBMG.Object;
using LBMG.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using LBMG.Map;

namespace LBMG.Object
{
    class TorchSystem
    {
        private const int TorchGenFreqOn = 33;
        private GameObjectSet _gObjSet;
        private Map.Map _map;

        public TorchSystem(GameObjectSet gameObjectSet, Map.Map map)
        {
            _gObjSet = gameObjectSet;
            _map = map;
        }

        public void SpreadWoodenSticksOnMap(int count)
        {
            var rnd = new Random();
            IEnumerable<Point> exploitableCoords = _map.GetCases(tm => tm.GetWoodenSticksCases());

            IEnumerable<Point> layingCoords = exploitableCoords
                .OrderBy(_ => rnd.Next())
                .Take(count);

            IEnumerable<WoodenSticks> layingWS = layingCoords
                .Select(coord => new WoodenSticks("ws", ObjectState.OnGround, coord));

            _gObjSet.Objects.AddRange(layingWS);
        }

        public void SpreadWoodenSticksOnMap()
        {
            int count = _map.GetCases(tm => tm.GetWoodenSticksCases()).Count() / TorchGenFreqOn;
            SpreadWoodenSticksOnMap(count);
        }

        public void Update(GameTime gameTime, KeyboardStateExtended kse, Character activeChar)
        {
            if (kse.WasKeyJustUp(Keys.Space))
            {
                AddTorch(activeChar.GetBehindCoordinates());
            }

            // Remove not burning torches
            var torches = _gObjSet.Objects.OfType<Torch>().ToArray();
            for (int i = 0; i < torches.Length; i++)
            {
                Torch torch = torches[i];
                if (!torches[i].IsBurning)
                    _gObjSet.Objects.Remove(torch);
            }
        }

        void AddTorch(Point coordinates)
        {
            var newTorch = new Torch("Torch", ObjectState.OnGround, coordinates);
            newTorch.IsBurning = true;
            _gObjSet.Objects.Add(newTorch);
        }
    }
}
