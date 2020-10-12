using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using LBMG.Player;
using LBMG.Map;
using System.Diagnostics;
using LBMG.UI;

namespace LBMG.Main
{
    public class LBMGGame : Game
    {
        readonly GraphicsDeviceManager gdm;
        private SpriteBatch _sb;
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

        public LBMGGame()
        {
            Content.RootDirectory = @"PipelineContent";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;

            gdm = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,
                IsFullScreen = true
            };
#if DEBUG
            gdm.IsFullScreen = false;
            gdm.PreferredBackBufferWidth = 800;
            gdm.PreferredBackBufferHeight = 600;
#endif
            Content.RootDirectory = "PipelineContent";

            Characters = new List<Character>
            {
                new Character("Peter", 70),
                new Character("Fred", 70)
            };
            Map = new Map.Map();
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
                "DialogBox/dialog_frame1"
            });

            ActivePLayer = 0;
        }

        protected override void Initialize()
        {
            _sb = new SpriteBatch(GraphicsDevice);
            MapDrawer.Initialize(GraphicsDevice, Content);
            CharacterDrawer.Initialize(GraphicsDevice, Content);
            UiDrawer.Initialize(Content);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            KeyboardStateExtended kse = KeyboardExtended.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                Exit();
            if (kse.WasKeyJustUp(Keys.C))                   // TEMP, will change with the timer later
                ActivePLayer = ActivePLayer == 0 ? 1 : 0;
            if (kse.WasKeyJustUp(Keys.L))                   // TEMP
            {
                Debug.WriteLine("Is about to write something...");
                UserInterface.DialogBox.Write(new List<int>{1, 2});
            }
            if (kse.WasKeyJustUp(Keys.N))                   // TEMP
                UserInterface.DialogBox.NextDialog();

            Controller.Update();
            ControlCharacter();
            CharacterDrawer.Update(gameTime);
            UiDrawer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color backgroundColor = new Color(1, 2, 11);
            GraphicsDevice.Clear(backgroundColor);
            base.Draw(gameTime);
            _sb.Begin();
            CharacterDrawer.Draw(_sb, gameTime);
            UiDrawer.Draw(_sb, gameTime);
            _sb.End();
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
