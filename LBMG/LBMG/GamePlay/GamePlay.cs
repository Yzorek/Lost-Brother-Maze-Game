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
        ActivePlayerTimer _activePlayerTimer = new ActivePlayerTimer();

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
            foreach (var ch in Characters) ch.SpawnAt(16, -16);

            Map = new Map.Map(Difficulty.Easy);
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

        public void Start()
        {
            Started = true;
            _activePlayerTimer.Start();
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
            
            _camera.ZoomIn(Constants.ZoomFact);
            _camera.Origin = Vector2.Zero;
            Debug.WriteLine(_camera.WorldToScreen(new Vector2(256, 256)));
            BackGroundMusic = cm.Load<Song>("Sounds/043 - Crystal Cave");
            CharacterDrawer.Initialize(gd, cm, window);
            MapDrawer.Initialize(gd, cm, window);
            UiDrawer.Initialize(cm, window);
            ObjectDrawer.Initialize(gd, cm, window);

#if DEBUG // So we can avoid redundant start menu
            Start();
#endif

            window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public void Update(GameTime gameTime, KeyboardStateExtended kse)
        {
            _activePlayerTimer.Update(gameTime);

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
            ObjectDrawer.UpdateObjects(gameTime, _camera);
            UiDrawer.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            MapDrawer.DrawBackLayer(gameTime, _camera, sb);
            CharacterDrawer.Draw(gameTime, _camera);
            MapDrawer.DrawFrontLayer(gameTime, _camera, sb);
            ObjectDrawer.DrawObjects(gameTime, _camera);
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
