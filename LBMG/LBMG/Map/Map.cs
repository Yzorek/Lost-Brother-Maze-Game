using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LBMG.Map
{
    public class Map
    {
        public Dictionary<Point, Piece> TiledMapsDictionary { get; set; }

        public Difficulty Difficulty { get; set; }

        public Map(Dictionary<Point, Piece> tiledMaps, Difficulty difficulty)
        {
            TiledMapsDictionary = tiledMaps;
            Difficulty = difficulty;
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
