using LBMG.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LBMG.Object
{
    public class GameObjectDrawer
    {

        private Vector2 _centerPos;
        private SpriteBatch _sb;
        private GameObjectSpriteFactory _spriteFactory;
        private GameObjectSet _set;

        public GameObjectDrawer(GameObjectSet set)
        {
            _set = set;
            _spriteFactory = new GameObjectSpriteFactory();
        }

        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            SetPosition(window);
            window.ClientSizeChanged += (s, e) => SetPosition(s as GameWindow);

            _spriteFactory.Initialize(cm);

            _sb = new SpriteBatch(gd);
        }

        private void SetPosition(GameWindow window)
        {
            _centerPos.X = window.ClientBounds.Width / 2;
            _centerPos.Y = window.ClientBounds.Height / 2;
        }

        public void UpdateObjects(GameTime gameTime, Camera<Vector2> camera)
        {
            for (int i = 0; i < _set.Objects.Count; i++)
            {
                _set.Objects[i].UpdateRectangle(gameTime, camera);
            }
        }

        public void DrawObjects(GameTime gameTime, Camera<Vector2> camera)
        {
            _sb.Begin(samplerState: SamplerState.PointClamp);

            for (int i = 0; i < _set.Objects.Count; i++)
            {
                GameObject obj = _set.Objects[i];
                if (_set.Objects[i].State == ObjectState.OnGround)
                {
                    Vector2 cdp = Entity.GetEntityDrawingPosByCamera(obj, camera, _centerPos);
                    _sb.Draw(_spriteFactory.GetGameObjectSprite(obj.Sprite), cdp, obj.Rect, Color.White, 
                        default, new Vector2(0, (float)obj.Rect.Height / 2), obj.DrawingScale, default, default);
                }
            }

            _sb.End();
        }
    }
}
