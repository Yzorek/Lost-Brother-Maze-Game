using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using LBMG.Tools;
using MonoGame.Extended.Tiled;
using MonoGame.Extended;

namespace LBMG.Player
{
    public class CharacterDrawer
    {
        public List<Character> Characters { get; set; }

        private readonly List<string> _texturePaths;
        private readonly List<Texture2D> _textures;
        private List<Rectangle> _rectangles;
        private int _activePlayer;
        private readonly Vector2 _playerPos;
        const float TileSize = Constants.TileSize;
        private float _counter;

        public CharacterDrawer(List<Character> characters, List<string> paths, List<Rectangle> rectangles)
        {
            Characters = characters;
            _texturePaths = paths;
            _rectangles = rectangles;
            _textures = new List<Texture2D>();
            _playerPos.X = (float) GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
            _playerPos.Y = (float) GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;

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

        public void Update(GameTime gameTime, Camera<Vector2> camera)
        {
            AnimateSprite();
            MoveToCase(gameTime);
            MoveCamera(gameTime, camera);
            if (_counter <= 0)
            {
                Characters[_activePlayer].IsMoving = false;
                Characters[_activePlayer].Move();
                AdjustCamera(camera);
                _counter = TileSize;
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime/*, Matrix transformMatrix*/)
        {
            sb.Draw(_textures[_activePlayer], _playerPos, _rectangles[_activePlayer], Color.White, default,
                new Vector2((float) _rectangles[_activePlayer].Width / 2, (float) _rectangles[_activePlayer].Height / 2), 1, default,
                default);
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
            rect.Size = new Point(50, AdjustSizeY(dir));
            _rectangles[_activePlayer] = rect;
        }

        private int GetYRectVal(Direction dir)
            => dir switch
            {
                Direction.Left => 72,
                Direction.Top => 216,
                Direction.Right => 144,
                Direction.Bottom => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };

        private int AdjustSizeY(Direction dir)
            => dir switch
            {
                Direction.Left => 69,
                Direction.Top => 60,
                Direction.Right => 69,
                Direction.Bottom => 69,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };

        private int GetXRectVal()
        {
            if (Characters[_activePlayer].IsMoving == false)
                return 50;
            if (_counter <= TileSize && _counter > TileSize - TileSize / 4)
                return -2;
            if (_counter <= TileSize - TileSize / 4 && _counter > TileSize - 2 * TileSize / 4)
                return 50;
            if (_counter <= TileSize - 2 * TileSize / 4 && _counter > TileSize - 3 * TileSize / 4)
                return 102;
            if (_counter <= TileSize - 3 * TileSize / 4)
                return 50;
            return -2;
        }

        private void MoveToCase(GameTime gameTime)
        {
            if (Characters[_activePlayer].IsMoving == true) // TODO Review this calculation
                _counter -= (Characters[_activePlayer].Speed * (float) gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void MoveCamera(GameTime gameTime, Camera<Vector2> camera)
        {
            if (Characters[_activePlayer].IsMoving == true)
            {
                switch (Characters[_activePlayer].Direction)
                {
                    case Direction.Left:
                        camera.Move(new Vector2(-Characters[_activePlayer].Speed * (float) gameTime.ElapsedGameTime.TotalSeconds, 0));
                        break;
                    case Direction.Top:
                        camera.Move(new Vector2(0, -Characters[_activePlayer].Speed * (float) gameTime.ElapsedGameTime.TotalSeconds));
                        break;
                    case Direction.Right:
                        camera.Move(new Vector2(Characters[_activePlayer].Speed * (float) gameTime.ElapsedGameTime.TotalSeconds, 0));
                        break;
                    case Direction.Bottom:
                        camera.Move(new Vector2(0, Characters[_activePlayer].Speed * (float) gameTime.ElapsedGameTime.TotalSeconds));
                        break;
                }
            }
        }

        private void AdjustCamera(Camera<Vector2> camera)
        {
            switch (Characters[_activePlayer].Direction)
            {
                case Direction.Left:
                    camera.Move(new Vector2(-_counter, 0));
                    break;
                case Direction.Top:
                    camera.Move(new Vector2(0, -_counter));
                    break;
                case Direction.Right:
                    camera.Move(new Vector2(_counter, 0));
                    break;
                case Direction.Bottom:
                    camera.Move(new Vector2(0, _counter));
                    break;
            }
        }
    }
}
