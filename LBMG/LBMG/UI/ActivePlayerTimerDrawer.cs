using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.UI
{
    class ActivePlayerTimerDrawer
    {
        private const string TimeSpanFormat = @"m\:ss";

        SpriteFont _font;
        ActivePlayerTimer _activePlayerTimer;

        public ActivePlayerTimerDrawer(ActivePlayerTimer activePlayerTimer, SpriteFont font)
        {
            _font = font;
            _activePlayerTimer = activePlayerTimer;
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            GraphicsDevice gd = sb.GraphicsDevice;
            string drawingTime = _activePlayerTimer.RemainingTime.ToString(TimeSpanFormat);
            Vector2 position = new Vector2(gd.Viewport.Width - 57, 5);
            sb.DrawString(_font, drawingTime, position, Color.DodgerBlue);
        }
    }
}
