using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using LBMG.Player;
using LBMG.Map;

namespace LBMG.Main
{
    public class LBMGGame : Game
    {
        readonly GraphicsDeviceManager gdm;
        private SpriteBatch _sb;
        private int _activePlayer = 0;
        private int ActivePLayer
        {
            get 
            {
                return _activePlayer;
            } 
            set 
            {
                _activePlayer = value;
                CharacterDrawer?.SetActivePlayer(value);
            } 
        }

        public Character Peter { get; set; }

        public Character Fred { get; set; }

        public Map.Map Map { get; set; }

        public MapDrawer MapDrawer { get; set; }

        public CharacterDrawer CharacterDrawer { get; set; }

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

            Peter = new Character("Peter");
            Fred = new Character("Fred");
            Map = new Map.Map();

            MapDrawer = new MapDrawer();

            CharacterDrawer = new CharacterDrawer(
                new List<Character> 
                {
                    Peter,
                    Fred
                },
                new List<string>
                {
                    "Characters/peter",
                    "Characters/fred"
                },
                new List<Rectangle>
                {
                    new Rectangle(0, 0, 24, 32),
                    new Rectangle(0, 0, 24, 32)
                });

            ActivePLayer = 0;
        }

        protected override void Initialize()
        {
            _sb = new SpriteBatch(GraphicsDevice);
            MapDrawer.Initialize(GraphicsDevice, Content);
            CharacterDrawer.Initialize(GraphicsDevice, Content);
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
            CharacterDrawer.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color backgroundColor = new Color(1, 2, 11);
            GraphicsDevice.Clear(backgroundColor);
            base.Draw(gameTime);
            _sb.Begin();
            CharacterDrawer.Draw(_sb, gameTime);
            _sb.End();
        }
    }
}
