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

        public GamePlay.GamePlay CurrentGame { get; set; }

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

            CurrentGame = new GamePlay.GamePlay();          // TEMP, later will be launched with the menu
        }

        protected override void Initialize()
        {
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
            if (CurrentGame.Started)
                CurrentGame.Update(gameTime, kse);
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
