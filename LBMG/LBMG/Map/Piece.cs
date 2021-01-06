using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LBMG.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace LBMG.Map
{
    public class Piece
    {
        public string Name { get; set; }

        public TiledMap TiledMap { get; set; }

        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapObjectLayer _collisionLayer;
        private TiledMapLayer _backLayer;
        private TiledMapLayer _frontLayer;
        private Point _drawingPos;

        public Point Location { get; set; }

        public Piece(TiledMap tiledMap, int locX, int locY)
        {
            TiledMap = tiledMap;
            Name = TiledMap.Name;
            Location = new Point(locX, locY);
        }

        public void Initialize(GraphicsDevice gd, GameWindow window)
        {
            window.ClientSizeChanged += Window_ClientSizeChanged;
            SetDrawingPosition(window);

            foreach (TiledMapLayer tml in TiledMap.Layers)
            {
                string[] name = tml.Name.ToLower().Split(' ', StringSplitOptions.None);

                if (name.Length == 2
                    && name[0] == "collision"
                    && name[1] == "layer"
                    && tml is TiledMapObjectLayer collisionLayer)
                {
                    _collisionLayer = collisionLayer;
                }
                else if (name.Length == 2 
                    && name[0] == "layer"
                    && int.TryParse(name[1], out int layerNumber))
                {
                    if (layerNumber == 1)
                        _backLayer = tml;
                    else if (layerNumber == 2)
                        _frontLayer = tml;
                }
            }

            _tiledMapRenderer = new TiledMapRenderer(gd, TiledMap);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            SetDrawingPosition(sender as GameWindow);
        }

        public void UpdateRenderer(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        public void DrawBackTiledMap(Camera<Vector2> camera, SpriteBatch spriteBatch) => DrawTiledMapLayer(camera, _backLayer, spriteBatch);
        public void DrawFrontTiledMap(Camera<Vector2> camera, SpriteBatch spriteBatch) => DrawTiledMapLayer(camera, _frontLayer, spriteBatch);

        private void DrawTiledMapLayer(Camera<Vector2> camera, TiledMapLayer layer, SpriteBatch spriteBatch)
        {
            Matrix matrix = camera.GetViewMatrix();

            matrix.Translation += new Vector3(_drawingPos.ToVector2(), 0);
            _tiledMapRenderer.Draw(layer, matrix);
        }

        private void SetDrawingPosition(GameWindow window)
        {
            int gpPosX = (Constants.TiledMapSizePixel + (int)(Constants.TiledMapSizePixel * Constants.ZoomFact)) * Location.X,
                gpPosY = (Constants.TiledMapSizePixel + (int)(Constants.TiledMapSizePixel * Constants.ZoomFact)) * Location.Y;

            gpPosX += window.ClientBounds.Width / 2;
            gpPosY += window.ClientBounds.Height / 2;

            _drawingPos = new Point(gpPosX, gpPosY);
        }

        public bool IsCollision(Point onPieceCoordinates)
        {
            return IsGenuineCollision(onPieceCoordinates)
                || IsGenuineCollision(onPieceCoordinates + new Point(1, 0)); // Check is not too much on the right actually
        }


        private bool IsGenuineCollision(Point onPieceCoordinates)
        {
            Point pos = new Point(onPieceCoordinates.X * Constants.TileSize + Constants.TileSize / 2, onPieceCoordinates.Y * Constants.TileSize + Constants.TileSize / 2);

            foreach (TiledMapObject tmObj in _collisionLayer.Objects)
            {
                if (pos.X >= tmObj.Position.X && pos.Y >= tmObj.Position.Y
                       && pos.X <= tmObj.Position.X + tmObj.Size.Width && pos.Y <= tmObj.Position.Y + tmObj.Size.Height)
                    return true;
            }

            return false;
        }
    }
}
