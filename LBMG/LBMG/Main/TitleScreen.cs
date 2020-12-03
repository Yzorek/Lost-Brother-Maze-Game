using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Main
{
    class TitleScreen
    {
        Vector2 _playButtonPos;
        Vector2 _playButtonTextPos;
        private SpriteFont _font;
        private Texture2D _buttonTexture;

        public event EventHandler PlayClick;

        public TitleScreen()
        { 
        }

        public void Initialize(ContentManager cm, GameWindow window)
        {
            _font = cm.Load<SpriteFont>("Fonts/MenuFont");
            _buttonTexture = cm.Load<Texture2D>("Menu/button");

            _playButtonPos = new Vector2(25, 25);
            _playButtonTextPos = new Vector2(35, 35);
        }

        public void Update(GameTime gameTime)
        {
            MouseStateExtended ms = MouseExtended.GetState();

            if (ms.WasButtonJustUp(MouseButton.Left) && 
                ms.X >= _playButtonPos.X && ms.Y >= _playButtonPos.Y
                && ms.X - _playButtonPos.X <= _buttonTexture.Width && ms.Y - _playButtonPos.Y <= _buttonTexture.Height)
            {

                PlayClick?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            sb.Draw(_buttonTexture, _playButtonPos, Color.White);
            sb.DrawString(_font, "Play", _playButtonTextPos, Color.DarkBlue);
        }

        // TODO Add Quit button properly
    }
}
