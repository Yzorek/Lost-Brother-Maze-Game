using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LBMG.UI
{
    public class UIDrawer
    {
        public DialogBoxDrawer DialogDrawer { get; set; }
        public UIDrawer(UI ui, List<string> texturePaths)
        {
            DialogDrawer = new DialogBoxDrawer(ui.DialogBox, texturePaths[0], new Rectangle(0, 0, ui.DialogBox.Size.X, ui.DialogBox.Size.Y));
        }

        public void Initialize(ContentManager cm, GameWindow window)
        {
            DialogDrawer.Initialize(cm, window);
        }

        public void Update(GameTime gameTime/*, Camera<Vector2> camera*/)
        {
            DialogDrawer.Update(gameTime);
        }

        public void Draw(SpriteBatch sb, GameTime gameTime/*, Matrix transformMatrix*/)
        {
            DialogDrawer.Draw(sb, gameTime);
        }
    }
}
