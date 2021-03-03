using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Object
{
    public enum GameObjectSprite
    {
        TorchLightened,
        Portal,
        Sign
    }

    public class GameObjectSpriteFactory
    {
        private Dictionary<GameObjectSprite, Texture2D> _goSpritesDict;

        public void Initialize(ContentManager content)
        {
            _goSpritesDict = new Dictionary<GameObjectSprite, Texture2D>
            {
                { GameObjectSprite.TorchLightened, content.Load<Texture2D>("Objects/torch_lightened") },
                { GameObjectSprite.Portal, content.Load<Texture2D>("Objects/portal") },
                { GameObjectSprite.Sign, content.Load<Texture2D>("Objects/sign") }
            };
        }

        public Texture2D GetGameObjectSprite(GameObjectSprite goSprite)
        {
            return _goSpritesDict[goSprite];
        }
    }
}
