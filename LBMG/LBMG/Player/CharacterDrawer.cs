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
        const int TileSize = 32;
        private double _counter;

        public CharacterDrawer(List<Character> characters, List<string> paths, List<Rectangle> rectangles)
        {
            Characters = characters;
            _texturePaths = paths;
            _rectangles = rectangles;
            _textures = new List<Texture2D>();
            _playerPos.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
            _playerPos.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
            _activePlayer = 0;
            _counter = TileSize;
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
            AnimateSprite();
            MoveToCase(gameTime);
            if (_counter <= 0)
            {
                Debug.WriteLine("Stop !");
                Characters[_activePlayer].IsMoving = false;
                _counter = TileSize;
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime/*, Matrix transformMatrix*/)
        {
            sb.Draw(_textures[_activePlayer], _playerPos, _rectangles[_activePlayer], Color.White);
        }

        public void SetActivePlayer(int val)
        {
            _activePlayer = val;
        }

        private void AnimateSprite()
        {
            Rectangle rect = _rectangles[_activePlayer];
            Direction dir = Characters[_activePlayer].Direction;

            rect.Y = GetYRectVal(dir);
            rect.X = GetXRectVal();
            _rectangles[_activePlayer] = rect;
        }

        private int GetYRectVal(Direction dir)
            => dir switch
            {
                Direction.Left => 69,
                Direction.Top => 207,
                Direction.Right => 138,
                Direction.Bottom => 0
            };

        private int GetXRectVal()
        {
            if (Characters[_activePlayer].IsMoving == false)
                return 50;
            if (_counter <= TileSize && _counter > TileSize - TileSize / 3)
            {
                Debug.WriteLine("rect 1");
                return 0;
            }
            if (_counter <= TileSize - TileSize / 3 && _counter > TileSize - 2 * TileSize / 3)
            {
                Debug.WriteLine("rect 2");
                return 50;
            }
            if (_counter <= TileSize - 2 * TileSize / 3)
            {
                Debug.WriteLine("rect 3");
                return 100;
            }
            return 0;
        }

        private void MoveToCase(GameTime gameTime)
        {
            if (Characters[_activePlayer].IsMoving == true)
                _counter -= (Characters[_activePlayer].Speed * gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
