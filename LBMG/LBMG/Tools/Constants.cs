using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Tools
{
    public static class Constants
    {
        /* ----------- DialogBox constants ----------- */

        public const int DBoxPaddingLeft = 8;
        public const int DBoxPaddingTop = 5;
        public const int DBoxPaddingRight = 8;
        public const int DBoxPaddingBottom = 5;

        /* ----------- Camera constants ----------- */

        public const float ZoomFact = .6F;

        /* ----------- Tile constants ----------- */

        public const int TileSize = 16;
        public const int TiledMapSizePixel = TiledMapSize * TileSize;
        public const int TiledMapSize = 32;
    }
}
