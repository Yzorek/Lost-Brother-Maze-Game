using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using LBMG.Tools;
using MonoGame.Extended.Tiled;

namespace LBMG.Map
{
    public class MapDrawer
    {
        private GameWindow _window;
        private Dictionary<Piece, Point> _drawingPositions;
        private Dictionary<TunnelMap, TiledMapRenderer> _tmRenderers;

        private IEnumerable<Piece> Pieces => Map.PiecesDictionary.Values;

        public Map Map { get; private set; }

        public MapDrawer(Map map = null)
        {
            Map = map;
        }

        public void Initialize(TunnelMapFactory mapFactory, GraphicsDevice gd, GameWindow window)
        {
            _tmRenderers = new Dictionary<TunnelMap, TiledMapRenderer>();
            foreach (TunnelMap tm in mapFactory.LoadedMaps)
            {
                TiledMapRenderer tmr = new TiledMapRenderer(gd, tm.TiledMap);
                _tmRenderers.Add(tm, tmr);
            }

            _window = window;
            window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public void LoadMap(Map map)
        {
            Map = map;
            _drawingPositions = new Dictionary<Piece, Point>();
            foreach (Piece p in Pieces)
                _drawingPositions.Add(p, Point.Zero);
            SetDrawingPositions(_window);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            SetDrawingPositions(sender as GameWindow);
        }

        public void Update(GameTime gameTime/*, Camera<Vector2> camera*/)
        {
            foreach (var tmRenderer in _tmRenderers.Values)
            {
                tmRenderer.Update(gameTime);
            }
        }

        public void DrawBackLayer(GameTime gameTime, Camera<Vector2> camera, SpriteBatch spriteBatch)
        {
            foreach (Piece piece in Pieces)
                DrawBackPiece(piece, camera, spriteBatch);

        }

        public void DrawFrontLayer(GameTime gameTime, Camera<Vector2> camera, SpriteBatch spriteBatch)
        {
            foreach (Piece piece in Pieces)
                DrawFrontPiece(piece, camera, spriteBatch);
        }

        private void DrawBackPiece(Piece piece, Camera<Vector2> camera, SpriteBatch spriteBatch) => DrawPieceLayer(piece, camera, piece.TunnelMap.BackLayer, spriteBatch);
        private void DrawFrontPiece(Piece piece, Camera<Vector2> camera, SpriteBatch spriteBatch) => DrawPieceLayer(piece, camera, piece.TunnelMap.FrontLayer, spriteBatch);

        private void DrawPieceLayer(Piece piece, Camera<Vector2> camera, TiledMapLayer layer, SpriteBatch spriteBatch)
        {
            Matrix matrix = camera.GetViewMatrix();

            matrix.Translation += new Vector3(_drawingPositions[piece].ToVector2(), 0);

            _tmRenderers[piece.TunnelMap].Draw(layer, matrix);
        }

        private void SetDrawingPositions(GameWindow window)
        {
            foreach (Piece piece in Pieces)
            {
                int gpPosX = (Constants.TiledMapSizePixel + (int)(Constants.TiledMapSizePixel * Constants.ZoomFact)) * piece.Location.X,
                gpPosY = (Constants.TiledMapSizePixel + (int)(Constants.TiledMapSizePixel * Constants.ZoomFact)) * piece.Location.Y;

                //_frontLayer.Offset = Location.ToVector2() * Constants.TiledMapSizePixel + new Vector2(camera.BoundingRectangle.Width, camera.BoundingRectangle.Height) / 2;
                //_backLayer.Offset = _frontLayer.Offset;

                gpPosX += window.ClientBounds.Width / 2;
                gpPosY += window.ClientBounds.Height / 2;

                _drawingPositions[piece] = new Point(gpPosX, gpPosY);
            }

        }
    }
}
