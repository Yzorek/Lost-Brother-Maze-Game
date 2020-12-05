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
        private int ActivePLayer
        {
            get => _activePlayer;
            set
            {
                _activePlayer = value;
                CharacterDrawer?.SetActivePlayer(value);
            }
        }

        static OrthographicCamera _camera;

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
                new Character("Peter", 70),
                new Character("Fred", 70)
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

            ActivePLayer = 0;
            Started = false;
        }

        public void Initialize(GraphicsDevice gd, ContentManager cm)
        {
            _camera ??= new OrthographicCamera(gd) { };

            _camera.ZoomIn(Constants.ZoomFact);
            MapDrawer.Initialize(gd, cm);
            CharacterDrawer.Initialize(gd, cm);
            UiDrawer.Initialize(cm);
            Started = true;
        }

        public void Update(GameTime gameTime, KeyboardStateExtended kse)
        {
            KeyboardState ks = Keyboard.GetState();
            if (kse.WasKeyJustUp(Keys.C))                   // TEMP, will change with the timer later
                ActivePLayer = ActivePLayer == 0 ? 1 : 0;
            if (kse.WasKeyJustUp(Keys.L))                   // TEMP
            {
                Debug.WriteLine("Is about to write something...");
                UserInterface.DialogBox.Write(5, new[] { "sud", "est" });
            }
            if (kse.WasKeyJustUp(Keys.N))                   // TEMP
                UserInterface.DialogBox.NextDialog();
            if (kse.WasKeyJustUp(Keys.T)) // TEMP
                TextBank.CurrentLanguage = Language.English;

            if (ks.IsKeyDown(Keys.Left))                                                                                        // TEMP
                _camera.Move(new Vector2(-Characters[ActivePLayer].Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0));   //
            else if (ks.IsKeyDown(Keys.Up))                                                                                     //
                _camera.Move(new Vector2(0, -Characters[ActivePLayer].Speed * (float)gameTime.ElapsedGameTime.TotalSeconds));   //
            else if (ks.IsKeyDown(Keys.Right))                                                                                  //
                _camera.Move(new Vector2(Characters[ActivePLayer].Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0));    //
            else if (ks.IsKeyDown(Keys.Down))                                                                                   //
                _camera.Move(new Vector2(0, Characters[ActivePLayer].Speed * (float)gameTime.ElapsedGameTime.TotalSeconds));    //

            Controller.Update();
            ControlCharacter();
            CharacterDrawer.Update(gameTime);
            MapDrawer.Update(gameTime);
            UiDrawer.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            CharacterDrawer.Draw(sb, gameTime);
            MapDrawer.Draw(gameTime, _camera);
            UiDrawer.Draw(sb, gameTime);
        }

        private void ControlCharacter()
        {
            if (Characters[ActivePLayer].IsMoving) return;
            SendMove();
        }

        private void SendMove()
        {
            Direction dir = Controller.Direction;

            Characters[ActivePLayer].Direction = dir;
            if (Controller.IsKeyPressed)                   //if (IsCollision() == false)       TODO : Add collision system
                Characters[ActivePLayer].IsMoving = true;
        }
    }
}
