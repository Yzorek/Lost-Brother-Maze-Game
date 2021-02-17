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

        static Camera<Vector2> _camera;
        ActivePlayerTimer _activePlayerTimer;
        ActivePlayerTimerDrawer _activePlayerTimerDrawer;
        TunnelMapFactory _tunnelMapFactory;

        private bool _found = false;
        private int OtherPlayer => ActivePlayer == 0 ? 1 : 0;

        public List<Character> Characters { get; set; }
        public Map.Map Map { get; set; }
        public MapDrawer MapDrawer { get; set; }
        public CharacterDrawer CharacterDrawer { get; set; }
        public Controller Controller { get; set; }
        public UI.UI UserInterface { get; set; }
        public UIDrawer UiDrawer { get; set; }
        public List<GameObject> Objects { get; set; }
        public GameObjectDrawer ObjectDrawer { get; set; }

        public bool Started { get; private set; }

        private Song BackGroundMusic { get; set; }

        public GamePlay()
        {
            Characters = new List<Character>
            {
                new Character("Peter", 160),
                new Character("Fred", 160)
            };

            float minutsTime = 1;
#if DEBUG
            minutsTime = 10;
#endif
            _activePlayerTimer = new ActivePlayerTimer(minutsTime * 60 * 1000);
            _activePlayerTimer.ChangeActivePlayer += ActivePlayerTimer_ChangeActivePlayer;
            Controller = new Controller();
            UserInterface = new UI.UI();
            Objects = new List<GameObject>
            {
                new Torch("Torch", ObjectState.OnGround, new Point(16, -16)),
                new Torch("Torch", ObjectState.OnGround, new Point(32, -16)),
                new Torch("Torch", ObjectState.OnGround, new Point(48, -16)),
                new Torch("Torch", ObjectState.OnGround, new Point(64, -16)),
                new Torch("Torch", ObjectState.OnGround, new Point(80, -16)),

            };

            InitDrawers();

            Started = false;
        }

        public void InitDrawers()
        {
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

            ObjectDrawer = new GameObjectDrawer(Objects, new List<string>
            {
                "Objects/torch_lightened",
                "Objects/torch_lightened",
                "Objects/torch_lightened",
                "Objects/torch_lightened",
                "Objects/torch_lightened"
            }, new List<Rectangle>
            {
                Constants.TorchRect(),
                Constants.TorchRect(),
                Constants.TorchRect(),
                Constants.TorchRect(),
                Constants.TorchRect()
            });
        }

        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            _camera ??= new OrthographicCamera(gd);

            _camera.ZoomIn(Constants.ZoomFact);
            _camera.Origin = Vector2.Zero;

            BackGroundMusic = cm.Load<Song>("Sounds/043 - Crystal Cave");
            CharacterDrawer.Initialize(gd, cm, window);

            _tunnelMapFactory = new TunnelMapFactory();
            _tunnelMapFactory.LoadTunnelMaps(cm);

            MapDrawer = new MapDrawer();
            MapDrawer.Initialize(_tunnelMapFactory, gd, window);

            UiDrawer.Initialize(cm, window);
            ObjectDrawer.Initialize(gd, cm, window);
            _activePlayerTimerDrawer = new ActivePlayerTimerDrawer(_activePlayerTimer, cm.Load<SpriteFont>("Fonts/myFont"));

#if DEBUG // So we can avoid redundant start menu
            StartGame();
#endif

            window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public void StartGame()
        {
            Map = LBMG.Map.Map.Create(Difficulty.Easy, _tunnelMapFactory, out Point[] characterSpawnPoints);

            MapDrawer.LoadMap(Map);

            // Spawn characters
            for (int i = 0; i < Characters.Count; i++)
            {
                Point spawningCoords = characterSpawnPoints[i];
                Debug.WriteLine(spawningCoords);
                Characters[i].SpawnAt(spawningCoords.X, spawningCoords.Y);
            }

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

        public void Update(GameTime gameTime, KeyboardStateExtended kse)
        {
            _activePlayerTimer.Update(gameTime);

#if DEBUG
            if (kse.WasKeyJustUp(Keys.C))
                ActivePlayer = OtherPlayer;
            if (kse.WasKeyJustUp(Keys.L))                   // TEMP
            {
                //UserInterface.DialogBox.Write(5, new[] { "sud", "est" });
                UserInterface.DialogBox.Write(3);
            }
            if (kse.WasKeyJustUp(Keys.N))                   // TEMP
                UserInterface.DialogBox.NextDialog();
            if (kse.WasKeyJustUp(Keys.T)) // TEMP
                TextBank.CurrentLanguage = Language.English;
#endif

            Controller.Update();
            if (!_found)
                ControlCharacter();
            CharacterDrawer.Update(gameTime, _camera);
            MapDrawer.Update(gameTime);
            ObjectDrawer.UpdateObjects(gameTime, _camera);
            UiDrawer.Update(gameTime);

            if (!_found && Characters[ActivePlayer].EncounteredCharacter(Characters[OtherPlayer]))
            {
                _found = true;
                UserInterface.DialogBox.Write(6);
                // Then waiting for enter
            }

            if (_found && kse.WasKeyJustUp(Keys.Enter))
            {  // Game finished
                UserInterface.DialogBox.NextDialog();
                QuitGameplayGoToMenu();
                return;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            MapDrawer.DrawBackLayer(gameTime, _camera, sb);
            CharacterDrawer.Draw(gameTime, _camera);
            MapDrawer.DrawFrontLayer(gameTime, _camera, sb);
            ObjectDrawer.DrawObjects(gameTime, _camera);
            UiDrawer.Draw(sb, gameTime);
            _activePlayerTimerDrawer.Draw(gameTime, sb);
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
