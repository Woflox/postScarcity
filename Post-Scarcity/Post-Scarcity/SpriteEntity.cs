using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Post_Scarcity
{
    public class SpriteEntity
    {
        public Texture2D texture;
        public Vector2 position;
        public Color color = Color.White;

        public SpriteEntity(Vector2 pos, string textureName)
        {
            texture = TextureLoader.LoadTexture(textureName);
            this.position = pos;
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Render()
        {
            if (position.X + texture.Width / 2 > Game1.instance.camera.boundary.Left
             && position.X - texture.Width / 2 < Game1.instance.camera.boundary.Right
             && position.Y > Game1.instance.camera.boundary.Top
             && position.Y - texture.Height < Game1.instance.camera.boundary.Bottom)
            {
                Game1.instance.spriteBatch.Draw(texture, new Vector2(position.X - texture.Width / 2, position.Y - texture.Height), color);
            }
        }
    }
}
