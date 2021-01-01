﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LBMG.Map;
using LBMG.Player;
using LBMG.Tools;
using LBMG.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
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

        public List<Character> Characters { get; set; }
        public Map.Map Map { get; set; }
        public MapDrawer MapDrawer { get; set; }
        public CharacterDrawer CharacterDrawer { get; set; }
        public Controller Controller { get; set; }
        public UI.UI UserInterface { get; set; }
        public UIDrawer UiDrawer { get; set; }

        public bool Started { get; private set; }

        private Song BackGroundMusic { get; set; }

        public GamePlay()
        {
            Characters = new List<Character>
            {
                new Character("Peter", 225),
                new Character("Fred", 225)
            };
            foreach (var ch in Characters) ch.SpawnAt(16, 16);

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

            Started = false;
        }

        public void Start()
        {
            Started = true;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(BackGroundMusic);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            GameWindow window = sender as GameWindow;

            UiDrawer.DialogDrawer.SetPixelPos(new Vector2(window.ClientBounds.Width / 2 - UserInterface.DialogBox.Size.X / 2,
                window.ClientBounds.Height - UserInterface.DialogBox.Size.Y - 20));
        }

        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            _camera ??= new OrthographicCamera(gd);

            ActivePlayer = 0;

            _camera.Origin = new Vector2(0, 0);
            _camera.ZoomIn(Constants.ZoomFact);
            BackGroundMusic = cm.Load<Song>("Sounds/043 - Crystal Cave");
            CharacterDrawer.Initialize(gd, cm, window);
            MapDrawer.Initialize(gd, cm, window);
            UiDrawer.Initialize(cm, window);

#if DEBUG // So we can avoid redundant start menu
            Start();
#endif

            window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public void Update(GameTime gameTime, KeyboardStateExtended kse)
        {
            if (kse.WasKeyJustUp(Keys.C))                   // TEMP, will change with the timer later
                ActivePlayer = ActivePlayer == 0 ? 1 : 0;
            if (kse.WasKeyJustUp(Keys.L))                   // TEMP
            {
                //UserInterface.DialogBox.Write(5, new[] { "sud", "est" });
                UserInterface.DialogBox.Write(3);
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
            if (Controller.IsKeyPressed)
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
