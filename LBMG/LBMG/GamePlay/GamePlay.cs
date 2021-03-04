using System;
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
using LBMG.Object;

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

        Camera<Vector2> _camera;
        ActivePlayerTimer _activePlayerTimer;
        ActivePlayerTimerDrawer _activePlayerTimerDrawer;
        TunnelMapFactory _tunnelMapFactory;
        bool _found = false;
        PortalSystem _portalSystem;

        private int OtherPlayer => ActivePlayer == 0 ? 1 : 0;

        public List<Character> Characters { get; set; }
        public Map.Map Map { get; set; }
        public MapDrawer MapDrawer { get; set; }
        public CharacterDrawer CharacterDrawer { get; set; }
        public Controller Controller { get; set; }
        public UI.UI UserInterface { get; set; }
        public UIDrawer UiDrawer { get; set; }
        public GameObjectDrawer ObjectDrawer { get; set; }
        public GameObjectSet GameObjectSet { get; set; }

        public bool Started { get; private set; }

        private Song BackGroundMusic { get; set; }

        public GamePlay()
        {
            Characters = new List<Character>
            {
                new Character("Peter", 160),
                new Character("Fred", 160)
            };

            double minutsTime = 1;
#if DEBUG
            minutsTime = 10;
#endif
            _activePlayerTimer = new ActivePlayerTimer(TimeSpan.FromMinutes(minutsTime));
            _activePlayerTimer.ChangeActivePlayer += ActivePlayerTimer_ChangeActivePlayer;

            Controller = new Controller();
            UserInterface = new UI.UI();

            GameObjectSet = new GameObjectSet();
            foreach (var @char in Characters)
                @char.Moved += GameObjectSet.Character_Moved;

            InstanceDrawers();

            Started = false;
        }

        public void InstanceDrawers()
        {
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

            ObjectDrawer = new GameObjectDrawer(GameObjectSet);
            GameObjectSet.Objects.AddRange(
                new List<GameObject> // TEMP
            {
                //new Portal("Portal", ObjectState.OnGround, new Point(16, -10)),
                //new Portal("Portal", ObjectState.OnGround, new Point(10, -20)),
                new Torch("Torch", ObjectState.OnGround, new Point(16, -16)),
                new Torch("Torch", ObjectState.OnGround, new Point(32, -16)),
                new Torch("Torch", ObjectState.OnGround, new Point(48, -16)),
                new Torch("Torch", ObjectState.OnGround, new Point(64, -16)),
                new Torch("Torch", ObjectState.OnGround, new Point(80, -16)),
            });
        }

        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            _camera = new OrthographicCamera(gd);

            _camera.ZoomIn(Constants.ZoomFact);
            _camera.Origin = Vector2.Zero;

            BackGroundMusic = cm.Load<Song>("Sounds/043 - Crystal Cave");
            CharacterDrawer.Initialize(gd, cm, window, _camera);

            _tunnelMapFactory = new TunnelMapFactory();
            _tunnelMapFactory.LoadTunnelMaps(cm);
            MapDrawer.Initialize(_tunnelMapFactory, gd, window);

            UiDrawer.Initialize(cm, window);
            ObjectDrawer.Initialize(gd, cm, window);

            _activePlayerTimerDrawer = new ActivePlayerTimerDrawer(_activePlayerTimer, cm.Load<SpriteFont>("Fonts/myFont"));

            window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public void StartGame()
        {
            Map = LBMG.Map.Map.Create(Difficulty.Easy, _tunnelMapFactory);

            MapDrawer.LoadMap(Map);
            _portalSystem = new PortalSystem(GameObjectSet, Map);
            _portalSystem.SpreadPortalsOnMap();

            // Spawn characters
            for (int i = 0; i < Characters.Count; i++)
            {
                Point spawningCoords = Map.SpawnCoordinates[i];
                Characters[i].SpawnAt(spawningCoords.X, spawningCoords.Y);
            }

#if DEBUG
            bool __spawnNear = false;

            if (__spawnNear)
            {
                Characters[0].SpawnAt(16, -16);
                Characters[1].SpawnAt(17, -20);
            }
#endif

            //{ // TEMP
            //    Portal prtl1 = new Portal("Portal", ObjectState.OnGround, Map.SpawnCoordinates[0] + new Point(-3, -2)),
            //                    prtl2 = new Portal("Portal", ObjectState.OnGround, Map.SpawnCoordinates[1] + new Point(-3, -2), prtl1);
            //    prtl1.DestinationPortal = prtl2;
            //    GameObjectSet.Objects.AddRange(new[] { prtl1, prtl2 });
            //}

            // TEMP
            Sign testsign = new Sign("Test Sign", ObjectState.OnGround, Characters[0].Coordinates + new Point(0, 3), UserInterface.DialogBox);
            GameObjectSet.Objects.Add(testsign);

            ActivePlayer = 0;

            Started = true;
            _activePlayerTimer.Start();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(BackGroundMusic);
        }

        public void QuitGameplayGoToMenu()
        {
            _found = false;
            Started = false;
            _activePlayerTimer.Stop();
            MediaPlayer.Stop();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            GameWindow window = sender as GameWindow;

            UiDrawer.DialogDrawer.SetPixelPos(new Vector2(window.ClientBounds.Width / 2 - UserInterface.DialogBox.Size.X / 2,
                window.ClientBounds.Height - UserInterface.DialogBox.Size.Y - 20));
        }

        private void ActivePlayerTimer_ChangeActivePlayer(object sender, EventArgs e)
        {
            ActivePlayer = OtherPlayer;
        }

        private void EnterPressed()
        {
            if (UserInterface.DialogBox.Active)
                UserInterface.DialogBox.NextDialog();
            else
            {
                Character character = Characters[ActivePlayer];
                GameObjectSet.Character_Clicked(character, character.GetFacingPoint());
            }
        }

        public void Update(GameTime gameTime, KeyboardStateExtended kse)
        {
            _activePlayerTimer.Update(gameTime);

#if DEBUG
            if (kse.WasKeyJustUp(Keys.C))
                ActivePlayer = OtherPlayer;
            if (kse.WasKeyJustUp(Keys.L))
            {
                //UserInterface.DialogBox.Write(5, new[] { "sud", "est" });
                UserInterface.DialogBox.Write(3);
            }
            if (kse.WasKeyJustUp(Keys.N))
                UserInterface.DialogBox.NextDialog();
            if (kse.WasKeyJustUp(Keys.T))
                TextBank.CurrentLanguage = Language.English;
#endif

            Controller.Update(kse);

            if (!UserInterface.DialogBox.Active)// Enter action already used with DialogBox
            {
                ControlCharacter();
            }

            if (kse.WasKeyJustDown(Keys.Enter))
                EnterPressed();

            CharacterDrawer.Update(gameTime, _camera);
            MapDrawer.Update(gameTime);
            ObjectDrawer.UpdateObjects(gameTime, _camera);
            UiDrawer.Update(gameTime);

            // TODO Handle this in a separate class, like CharacterInteraction
            if (!_found && Characters[ActivePlayer].EncounteredCharacter(Characters[OtherPlayer]))
            {
                _found = true;
                UserInterface.DialogBox.Write(6);
                // Then waiting for enter
            }

            if (_found && !UserInterface.DialogBox.Active)
            {  // Game finished
                QuitGameplayGoToMenu();
                return;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            MapDrawer.DrawBackLayer(gameTime, _camera, sb);
            ObjectDrawer.DrawObjects(gameTime, _camera);
            CharacterDrawer.Draw(gameTime, _camera);
            MapDrawer.DrawFrontLayer(gameTime, _camera, sb);
            UiDrawer.Draw(sb, gameTime);
            _activePlayerTimerDrawer.Draw(gameTime, sb);
        }

        private void ControlCharacter()
        {
            if (Characters[ActivePlayer].IsMoving)
                return;

            SendMove();
        }

        private void SendMove()
        {
            if (Controller.IsKeyPressed)
            {
                Direction dir = Controller.Direction;
                Characters[ActivePlayer].Direction = dir;

                if (!Map.IsCollision(Characters[ActivePlayer].GetFacingPoint(), GameObjectSet))
                {
                    Characters[ActivePlayer].IsMoving = true;
                }
            }
        }
    }
}
