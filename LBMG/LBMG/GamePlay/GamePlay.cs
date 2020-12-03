using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LBMG.Map;
using LBMG.Player;
using LBMG.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            //Map = new Map.Map();
            Controller = new Controller();
            UserInterface = new UI.UI();

            MapDrawer = new MapDrawer();

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

        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            MapDrawer.Initialize(gd, cm);
            CharacterDrawer.Initialize(gd, cm, window);
            UiDrawer.Initialize(cm, window);
            Started = false;// true;
        }

        public void Update(GameTime gameTime, KeyboardStateExtended kse)
        {
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

            Controller.Update();
            ControlCharacter();
            CharacterDrawer.Update(gameTime);
            UiDrawer.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            CharacterDrawer.Draw(sb, gameTime);
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
