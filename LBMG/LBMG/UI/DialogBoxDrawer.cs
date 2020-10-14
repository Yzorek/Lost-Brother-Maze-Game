﻿using System;
using System.Collections.Generic;
using System.Text;
using LBMG.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LBMG.UI
{
    public class DialogBoxDrawer
    {
        public DialogBox DialogBox { get; set; }

        private Texture2D _boxTexture;
        private readonly string _boxTexturePath;
        private readonly Rectangle _rectangle;
        private readonly Vector2 _boxPos;
        private readonly Vector2 _textPos;
        private SpriteFont _font;

        public DialogBoxDrawer(DialogBox dialogBox, string texturePath, Rectangle rectangle)
        {
            DialogBox = dialogBox;
            _boxTexturePath = texturePath;
            _rectangle = rectangle;
            _boxPos.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - DialogBox.Size.X / 2;
            _boxPos.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - DialogBox.Size.Y - 20;
            _textPos.X = _boxPos.X + Constants.DBoxPaddingLeft;
            _textPos.Y = _boxPos.Y + Constants.DBoxPaddingTop;
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
                sb.DrawString(_font, DialogBox.TextWritten[DialogBox.CurrentTextIndex], _textPos, Color.White);
            }
        }
    }
}
