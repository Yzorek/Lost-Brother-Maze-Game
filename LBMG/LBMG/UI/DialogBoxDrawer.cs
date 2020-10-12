using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LBMG.UI
{
    public class DialogBoxDrawer
    {
        public DialogBox DialogBox { get; set; }

        private Texture2D _boxTexture;
        private string _boxTexturePath;
        private Rectangle _rectangle;
        private Vector2 _boxPos;
        private SpriteFont _font;

        public DialogBoxDrawer(DialogBox dialogBox, string texturePath, Rectangle rectangle)
        {
            DialogBox = dialogBox;
            _boxTexturePath = texturePath;
            _rectangle = rectangle;
            _boxPos.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - DialogBox.Size.X / 2;
            _boxPos.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - DialogBox.Size.Y - 20;
        }

        public void Initialize(ContentManager cm)
        {
            _boxTexture = cm.Load<Texture2D>(_boxTexturePath);
            _font = cm.Load<SpriteFont>(DialogBox.FontPath);
        }

        public void Update(GameTime gameTime/*, Camera<Vector2> camera*/)
        {

        }

        public void Draw(SpriteBatch sb, GameTime gameTime/*, Matrix transformMatrix*/)
        {
            if (DialogBox.Visible)
            {
                sb.Draw(_boxTexture, _boxPos, _rectangle, Color.White);
                sb.DrawString(_font, DialogBox.TextWritten[DialogBox.CurrentTextIndex], _boxPos, Color.White);
            }
        }
    }
}
