using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LBMG.Map;
using LBMG.Player;
using LBMG.Tools;
using LBMG.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace LBMG.GamePlay
{
    public class GamePlay
    {
        private int _activePlayer = 0;
        private int ActivePlayer
        {
            get => _activePlayer;
            set
            {
                _activePlayer = value;
                CharacterDrawer?.SetActivePlayer(value, _camera);
            }
        }

        static OrthographicCamera _camera;

        public Vector2 CameraPos { get; set; }
        public List<Character> Characters { get; set; }
        public Map.Map Map { get; set; }
        public MapDrawer MapDrawer { get; set; }
        public CharacterDrawer CharacterDrawer { get; set; }
        public Controller Controller { get; set; }
        public UI.UI UserInterface { get; set; }
        public UIDrawer UiDrawer { get; set; }
        public bool Started { get; set; }

        public GamePlay()
        {
            Characters = new List<Character>
            {
                new Character("Peter", 175),
                new Character("Fred", 250)
            };
            Map = new Map.Map(Difficulty.Easy);
            Controller = new Controller();
            UserInterface = new UI.UI();

            MapDrawer = new MapDrawer(Map);

            CharacterDrawer = new CharacterDrawer(Characters, new List<string>
            {
                "Characters/peter",
                "Characters/fred"
            }, new List<Rectangle>
            {
                new Rectangle(0, 0, 50, 69),
                new Rectangle(0, 0, 50, 69)
            });

            UiDrawer = new UIDrawer(UserInterface, new List<string>
            {
                "DialogBox/dialog_box"
            });

            CameraPos = new Vector2(0);
            Started = false;

#if DEBUG
            Started = true;
#endif
        }

        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            _camera ??= new OrthographicCamera(gd);

            ActivePlayer = 0;

            _camera.ZoomIn(Constants.ZoomFact);
            _camera.Origin = new Vector2(0, 0);

            CharacterDrawer.Initialize(gd, cm, window);
            MapDrawer.Initialize(gd, cm, window);
            UiDrawer.Initialize(cm, window);
        }

        public void Update(GameTime gameTime, KeyboardStateExtended kse)
        {
            if (kse.WasKeyJustUp(Keys.C))                   // TEMP, will change with the timer later
            {
                ActivePlayer = ActivePlayer == 0 ? 1 : 0;
            }
            if (kse.WasKeyJustUp(Keys.H))
            {
                //Characters[ActivePlayer].Position += new Point(20, 0);
            }
            if (kse.WasKeyJustUp(Keys.L))                   // TEMP
            {
                Debug.WriteLine("Is about to write something...");
                UserInterface.DialogBox.Write(5, new[] { "sud", "est" });
            }
            if (kse.WasKeyJustUp(Keys.N))                   // TEMP
                UserInterface.DialogBox.NextDialog();
            if (kse.WasKeyJustUp(Keys.T)) // TEMP
                TextBank.CurrentLanguage = Language.English;

            Controller.Update();
            ControlCharacter();
            CharacterDrawer.Update(gameTime, _camera);
            MapDrawer.Update(gameTime);
            UiDrawer.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            MapDrawer.DrawBackLayer(gameTime, _camera, sb);
            CharacterDrawer.Draw(gameTime, _camera);
            MapDrawer.DrawFrontLayer(gameTime, _camera, sb);
            UiDrawer.Draw(sb, gameTime);
        }

        private void ControlCharacter()
        {
            if (Characters[ActivePlayer].IsMoving) return;
            SendMove();
        }

        private void SendMove()
        {
            if (Controller.IsKeyPressed)                    //if (IsCollision() == false)       TODO : Add collision system
            {
                Direction dir = Controller.Direction;
                Characters[ActivePlayer].Direction = dir;

                if (!Map.IsCollision(Characters[ActivePlayer].GetFacingPoint()))
                {
                    Characters[ActivePlayer].IsMoving = true;
                }
            }
        }
    }
}
