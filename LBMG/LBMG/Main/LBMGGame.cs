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
using System.Runtime.InteropServices;


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

        public GamePlay.GamePlay CurrentGame { get; set; }

        public LBMGGame()
        {
            Content.RootDirectory = @"PipelineContent";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;

            gdm = new GraphicsDeviceManager(this);

            Content.RootDirectory = "PipelineContent";

            CurrentGame = new GamePlay.GamePlay();          // TEMP, later will be launched with the menu
        }

        protected override void Initialize()
        {
#if !DEBUG
            gdm.IsFullScreen = true;
            gdm.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            gdm.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            gdm.ApplyChanges();
#endif

            _sb = new SpriteBatch(GraphicsDevice);
            CurrentGame.Initialize(GraphicsDevice, Content);    // TEMP, later will be launched with the menu
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
                UserInterface.DialogBox.Write(5, new[]{"sud", "est"});
            }
            if (kse.WasKeyJustUp(Keys.N))                   // TEMP
                UserInterface.DialogBox.NextDialog();
            if (kse.WasKeyJustUp(Keys.T)) // TEMP
                TextBank.CurrentLanguage = Language.English;

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
            if (CurrentGame.Started)
                CurrentGame.Draw(gameTime, _sb);
            _sb.End();
        }
    }
}
