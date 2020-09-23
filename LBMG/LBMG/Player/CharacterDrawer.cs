using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace LBMG.Player
{
    public class CharacterDrawer
    {
        public List<Character> Characters { get; set; }

        private readonly List<string> _texturePaths;
        private readonly List<Texture2D> _textures;
        private List<Rectangle> _rectangles;
        private int _activePlayer;
        private Vector2 _playerPos;

        public CharacterDrawer(List<Character> characters, List<string> paths, List<Rectangle> rectangles)
        {
            Characters = characters;
            _texturePaths = paths;
            _rectangles = rectangles;
            _textures = new List<Texture2D>();
            _playerPos.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
            _playerPos.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
            _activePlayer = 0;
        }

        public void Initialize(GraphicsDevice gd, ContentManager cm)
        {
            foreach (string path in _texturePaths)
            {
                Texture2D text = cm.Load<Texture2D>(path);
                _textures.Add(text);
            }
        }

        public void Update(GameTime gameTime/*, Camera<Vector2> camera*/)
        {

        }

        public void Draw(SpriteBatch sb, GameTime gameTime/*, Matrix transformMatrix*/)
        {
            sb.Draw(_textures[_activePlayer], _playerPos, _rectangles[_activePlayer], Color.White);
        }

        public void SetActivePlayer(int val)
        {
            _activePlayer = val;
        }
    }
}
