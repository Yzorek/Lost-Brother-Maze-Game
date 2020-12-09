using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Gui.Controls;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Main
{
    class TitleScreen
    {

        public event EventHandler PlayClick;
        public event EventHandler QuitClick;
        public event EventHandler<SettingsChangedEventArgs> SettingsChanged;

        private GuiSystem _guiSys;

        public TitleScreen()
        {

        }

        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            var viewportAdapter = new DefaultViewportAdapter(gd);
            var guiRenderer = new GuiSpriteBatchRenderer(gd, () => Matrix.Identity);
            var font = cm.Load<BitmapFont>("Fonts/MenuFontBmp");
            BitmapFont.UseKernings = false;
            Skin.CreateDefault(font);

            var controlsScrn = GenerateControls();

            _guiSys = new GuiSystem(viewportAdapter, guiRenderer) { ActiveScreen = controlsScrn };
            window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _guiSys.ClientSizeChanged();
        }

        public void Update(GameTime gameTime)
        {
            _guiSys.Update(gameTime);
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            _guiSys.Draw(gameTime);
        }

        Screen GenerateControls()
        {
            #region Main Menu
            Thickness margin1 = new Thickness(20, 20, 0, 0);
            int btnWidth = 235, btnHeight = 105;

            Label titleLabel =
                    new Label("LOST BROTHER MAZE GAME\nVersion Indev")
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = margin1
                    };

            Button playBtn = new Button
            {
                Content = "Play",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = margin1,
                Width = btnWidth,
                Height = btnHeight
            },
            settingsBtn = new Button
            {
                Content = "Settings",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = margin1,
                Width = btnWidth,
                Height = btnHeight
            },
            quitBtn = new Button
            {
                Content = "Quit",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = margin1,
                Width = btnWidth,
                Height = btnHeight
            };

            var mainLayout = new StackPanel
            {
                Margin = new Thickness(0),
                BackgroundColor = new Color(51, 25, 0),
                Items = { titleLabel, playBtn, settingsBtn, quitBtn }
            };
            #endregion

            // Handlable main layout and return screen object here
            var switchableLayoutControl = new ContentControl { Content = mainLayout, Padding = new Thickness(0) };
            var scrn = new Screen { Content = switchableLayoutControl };

            #region Settings Menu

            var doneBtn = new Button
            {
                Content = "Done",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 20, 20),

            };
            var fullScreenCheckBox = new CheckBox
            {
                Content = "Full screen enabled",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = margin1,

            };

            var settingsLayout = new UniformGrid
            {
                BackgroundColor = Color.Black,
                Rows = 2, Columns = 1,
                Items =
                {
                    new StackPanel
                    {
                        Items = {
                            new Label("SETTINGS\n---")
                            {
                                Margin = margin1
                            },
                            fullScreenCheckBox 
                        }
                    },
                    doneBtn,
                }
            };
            #endregion

            #region Events assignment
            playBtn.Clicked += (s, e) => PlayClick?.Invoke(this, EventArgs.Empty);
            quitBtn.Clicked += (s, e) => QuitClick?.Invoke(this, EventArgs.Empty);
            settingsBtn.Clicked += (s, e) => switchableLayoutControl.Content = settingsLayout;

            doneBtn.Clicked += (s, e) => 
            {
                SettingsChanged?.Invoke(this, new SettingsChangedEventArgs
                {
                    FullScreenEnabled = fullScreenCheckBox.IsChecked
                });
                switchableLayoutControl.Content = mainLayout;
            };
            #endregion

            return scrn;
        }
    }
}
