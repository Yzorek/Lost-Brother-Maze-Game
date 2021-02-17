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
        private Vector2 _centerPos;
        const float TileSize = Constants.TileSize;
        private SpriteBatch _sb;
        private float _counter;
        
        public CharacterDrawer(List<Character> characters, List<string> paths, List<Rectangle> rectangles)
        {
            Characters = characters;
            _texturePaths = paths;
            _rectangles = rectangles;
            _textures = new List<Texture2D>();
        }


        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            SetPosition(window);
            window.ClientSizeChanged += (s, e) => SetPosition(s as GameWindow);

            _activePlayer = 0;
            _counter = TileSize;
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

        public void Update(GameTime gameTime, Camera<Vector2> camera)
        {
            AnimateSprite();
            MoveToCase(gameTime);
            MoveCamera(gameTime, camera);
            if (_counter <= 0)
            {
                Characters[_activePlayer].IsMoving = false;
                Characters[_activePlayer].Move();
                Characters[_activePlayer].MoveState = (Characters[_activePlayer].MoveState == 1) ? 2 : 1;
                AdjustCamera(camera);
                _counter = TileSize;
            }
        }

        public void Draw(GameTime gameTime, Camera<Vector2> camera)
        {
            _sb.Begin(samplerState: SamplerState.PointClamp);

            if (ActivePlayerOverInactive()) 
            {
                DrawActiveChar(camera);
                DrawInactiveChar(camera);
            }
            else
            {
                DrawInactiveChar(camera);
                DrawActiveChar(camera);
            }
           

            _sb.End();
        }

        private bool ActivePlayerOverInactive()
        {
          return   Characters[_activePlayer].Coordinates.Y > Characters[_activePlayer == 0 ? 1 : 0].Coordinates.Y;
        }

        private void DrawActiveChar(Camera<Vector2> camera)
        {
            _sb.Draw(_textures[_activePlayer], _centerPos, _rectangles[_activePlayer], Color.White, default,
                new Vector2(0, (float)_rectangles[_activePlayer].Height / 2), 1, default,
                default);
        }

        private void DrawInactiveChar(Camera<Vector2> camera)
        {
            // Drawing inactive players
            for (int i = 0; i < Characters.Count; i++)
            {
                if (i == _activePlayer) // Don't write our same player twice
                    continue;

                Vector2 cdp = Entity.GetEntityDrawingPosByCamera(Characters[i], camera, _centerPos);
                _sb.Draw(_textures[i], cdp, _rectangles[i], Color.White, default, new Vector2(0, (float)_rectangles[_activePlayer].Height / 2), 1, default, default);
            }
        }

        public void SetActivePlayer(int val, Camera<Vector2> camera)
        {
            Characters[_activePlayer].IsMoving = false;
            _activePlayer = val;

            SetCameraPosToCharacterCoords(camera);
        }

        private void AnimateSprite()
        {
            // TODO Update too inactive to right.
            for (int pl = 0; pl < Characters.Count; pl++)
            {
                Rectangle rect = _rectangles[pl];
                Direction dir = Characters[pl].Direction;

                rect.Y = GetYRectVal(dir);
                rect.X = GetXRectVal(pl);
                rect.Size = new Point(50, AdjustSizeY(dir));
                _rectangles[pl] = rect;
            }

            
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

        private int GetXRectVal(int player)
        {
            if (Characters[player].IsMoving == false)
                return 50;
            if (Characters[player].MoveState == 1)
            {
                if (_counter <= TileSize && _counter > TileSize - TileSize / 2)
                    return -2;
                if (_counter <= TileSize - TileSize / 2)
                    return 50;
            }
            if (Characters[player].MoveState == 2)
            {
                if (_counter <= TileSize && _counter > TileSize - TileSize / 2)
                    return 102;
                if (_counter <= TileSize - TileSize / 2)
                    return 50;
            }
            return -2;
        }

        private void MoveToCase(GameTime gameTime)
        {
            if (Characters[_activePlayer].IsMoving == true)
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

        private void SetCameraPosToCharacterCoords(Camera<Vector2> camera)
        {
            Point charPos = Characters[_activePlayer].Coordinates;
            camera.Position = Entity.GetPixelPosFromCoordinates(charPos);
        }
    }
}
