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
        private TiledMapObjectLayer _collisionLayer;
        private TiledMapLayer _backLayer;
        private TiledMapLayer _frontLayer;

        public Point Position { get; set; }

        public Piece(TiledMap tiledMap, int posX, int posY)
        {
            TiledMap = tiledMap;
            Name = TiledMap.Name;
            Position = new Point(posX, posY);
        }

        public void Initialize(GraphicsDevice gd)
        {
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

        public void UpdateRenderer(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        public void DrawBackTiledMap(Camera<Vector2> camera, SpriteBatch spriteBatch) => DrawTiledMapLayer(camera, _backLayer, spriteBatch);
        public void DrawFrontTiledMap(Camera<Vector2> camera, SpriteBatch spriteBatch) => DrawTiledMapLayer(camera, _frontLayer, spriteBatch);

        private void DrawTiledMapLayer(Camera<Vector2> camera, TiledMapLayer layer, SpriteBatch spriteBatch)
        {
            Matrix matrix = camera.GetViewMatrix();

            matrix.Translation += new Vector3(Position.ToVector2(), 0);
            _tiledMapRenderer.Draw(layer, matrix);
        }
    }
}
