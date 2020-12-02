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
