using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;

namespace LBMG.Map
{
    public class MapDrawer
    {
        public Map Map { get; set; }

        public MapDrawer(Map map)
        {
            Map = map;
        }

        public void Initialize(GraphicsDevice gd, ContentManager cm, GameWindow window)
        {
            Map.LoadMap(gd, cm, window);
        }

        public void Update(GameTime gameTime/*, Camera<Vector2> camera*/)
        {
            foreach (Piece piece in Map.TiledMapsDictionary.Values)
            {
                piece.UpdateRenderer(gameTime);
            }
        }

        public void DrawBackLayer(GameTime gameTime, Camera<Vector2> camera, SpriteBatch spriteBatch)
        {
            foreach (Piece piece in Map.TiledMapsDictionary.Values)
                piece.DrawBackTiledMap(camera, spriteBatch);
        }

        public void DrawFrontLayer(GameTime gameTime, Camera<Vector2> camera, SpriteBatch spriteBatch)
        {
            foreach (Piece piece in Map.TiledMapsDictionary.Values)
                piece.DrawFrontTiledMap(camera, spriteBatch);
        }
    }
}
