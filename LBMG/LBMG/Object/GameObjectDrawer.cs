using LBMG.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Object
{
    public class GameObjectDrawer
    {
        public List<GameObject> Objects { get; set; }

        private readonly List<string> _texturePaths;
        private readonly List<Texture2D> _textures;
        private List<Rectangle> _rectangles;
        private Vector2 _centerPos;
        private SpriteBatch _sb;

        public GameObjectDrawer(List<GameObject> objects, List<string> paths, List<Rectangle> rectangles)
        {
            Objects = objects;
            _texturePaths = paths;
            _rectangles = rectangles;
            _textures = new List<Texture2D>();
        }


        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            SetPosition(window);
            window.ClientSizeChanged += (s, e) => SetPosition(s as GameWindow);

            _sb = new SpriteBatch(gd);

            foreach (string path in _texturePaths)
            {
                Texture2D text = cm.Load<Texture2D>(path);
                _textures.Add(text);
            }
        }

        private void SetPosition(GameWindow window)
        {
            _centerPos.X = window.ClientBounds.Width / 2;
            _centerPos.Y = window.ClientBounds.Height / 2;
        }

        public void UpdateObjects(GameTime gameTime, Camera<Vector2> camera)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].Update(gameTime, camera, _rectangles[i]);
            }
        }

        public void DrawObjects(GameTime gameTime, Camera<Vector2> camera)
        {
            _sb.Begin(samplerState: SamplerState.PointClamp);

            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].State == ObjectState.OnGround)
                {
                    Vector2 cdp = Entity.GetEntityDrawingPosByCamera(Objects[i], camera, _centerPos);
                    _sb.Draw(_textures[i], cdp, _rectangles[i], Color.White, default, new Vector2(0, (float)_rectangles[i].Height / 2), 1, default, default);
                }
            }

            _sb.End();
        }
    }
}
