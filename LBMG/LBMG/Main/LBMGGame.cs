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

        readonly GraphicsDeviceManager _gdm;
        private SpriteBatch _sb;
        TitleScreen _titleScreen;

        public GamePlay.GamePlay CurrentGame { get; set; }

        public LBMGGame()
        {
            Content.RootDirectory = @"PipelineContent";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;

            _gdm = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
#if !DEBUG
            SetFullScreen(true);
#else
            SetFullScreen(false);
#endif

            _titleScreen = new TitleScreen();
            _titleScreen.PlayClick += (s, e) => CurrentGame.Started = true;
            _titleScreen.QuitClick += (s, e) => Exit();
            _titleScreen.SettingsChanged += SettingsChanged;
            CurrentGame = new GamePlay.GamePlay();         

            _sb = new SpriteBatch(GraphicsDevice);
            _titleScreen.Initialize(GraphicsDevice, Content, Window);
            CurrentGame.Initialize(GraphicsDevice, Content, Window);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            KeyboardStateExtended kse = KeyboardExtended.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                Exit();
            if (CurrentGame.Started)
                CurrentGame.Update(gameTime, kse);
            else
                _titleScreen.Update(gameTime);
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
            else
                _titleScreen.Draw(_sb, gameTime);

            _sb.End();
        }

        private void SettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            SetFullScreen(e.FullScreenEnabled);
        }

        void SetFullScreen(bool enabled)
        {
            if (enabled == _gdm.IsFullScreen) // Actually, if the window was already not fullscreen, we'd better not change anything 
                return;

            if (enabled)
            {
                _gdm.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _gdm.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                _gdm.PreferredBackBufferWidth = 800;
                _gdm.PreferredBackBufferHeight = 600;
            }

            _gdm.IsFullScreen = _gdm.IsFullScreen;
            _gdm.ApplyChanges();
        }
    }
}
