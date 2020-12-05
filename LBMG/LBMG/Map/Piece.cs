using System;
using System.Collections.Generic;
using System.Text;
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

        public Point Position { get; set; }

        public Piece(GraphicsDevice gd, TiledMap tiledMap, int posX, int posY)
        {
            TiledMap = tiledMap;
            Name = TiledMap.Name;
            Position = new Point(posX, posY);
            _tiledMapRenderer = new TiledMapRenderer(gd, TiledMap);
        }

        public void UpdateRenderer(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        public void DrawTiledMap(Camera<Vector2> camera)
        {
            Matrix matrix = camera.GetViewMatrix();

            matrix.Translation = new Vector3(matrix.Translation.X + Position.X, matrix.Translation.Y + Position.Y, 0);
            _tiledMapRenderer.Draw(matrix);
        }
    }
}
