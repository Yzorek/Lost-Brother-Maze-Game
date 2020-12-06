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
        class Button
        {
            public Vector2 Position { get; set; }
            public string Text { get; set; }
            public Vector2 TextPos { get; set; }
            public Texture2D Texture { get; set; }
            public SpriteFont Font { get; set; }
            public EventHandler ClickedEvent { get; set; }

            public Button(Vector2 position, string text, Vector2 textPos, Texture2D texture, SpriteFont font, EventHandler clicked)
            {
                TextPos = textPos;
                Texture = texture;
                Position = position;
                Text = text;
                Font = font;
                ClickedEvent += clicked;
            }
        }

        private SpriteFont _font;
        private Texture2D _buttonTexture;

        Button[] _buttons;

        public event EventHandler PlayClick;
        public event EventHandler QuitClick;

        public TitleScreen()
        {

        }

        public void Initialize(ContentManager cm, GameWindow window)
        {
            _font = cm.Load<SpriteFont>("Fonts/MenuFont");
            _buttonTexture = cm.Load<Texture2D>("Menu/button");

            _buttons = new Button[]
            {
                new Button(new Vector2(25, 25), "Play", new Vector2(35, 35), _buttonTexture, _font, PlayClick),
                new Button(new Vector2(25, 165), "Quit", new Vector2(35, 175), _buttonTexture, _font, QuitClick),
            };
        }

        public void Update(GameTime gameTime)
        {
            MouseStateExtended ms = MouseExtended.GetState();

            foreach (Button b in _buttons)
            {
                if (ms.WasButtonJustUp(MouseButton.Left) &&
                    ms.X >= b.Position.X && ms.Y >= b.Position.Y
                    && ms.X - b.Position.X <= b.Texture.Width && ms.Y - b.Position.Y <= b.Texture.Height)
                {
                    b.ClickedEvent?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            foreach (Button b in _buttons)
            {
                sb.Draw(b.Texture, b.Position, Color.White);
                sb.DrawString(b.Font ,b.Text, b.TextPos, Color.DarkBlue);
            }
        }

        // TODO Add Quit button properly
    }
}
