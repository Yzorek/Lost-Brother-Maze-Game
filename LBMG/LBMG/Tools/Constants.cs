﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace LBMG.Tools
{
    public static class Constants
    {
        /* ----------- DialogBox constants ----------- */

        public const int DBoxPaddingLeft = 20;
        public const int DBoxPaddingTop = 15;
        public const int DBoxPaddingRight = 20;
        public const int DBoxPaddingBottom = 15;

        /* ----------- Camera constants ----------- */

        public const float ZoomFact = .6F;

        /* ----------- Tile constants ----------- */

        public const int TileSize = 16;
        public const int TiledMapSizePixel = TiledMapSize * TileSize; // 32 * 16 = 512
        public const int TiledMapSize = 32;

        /* ----------- Objects constants ----------- */

        public static Rectangle TorchRect()
        {
            return new Rectangle(0, 0, 37, 125);
        }
    }
}
