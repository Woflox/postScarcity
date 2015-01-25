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
    public class Ladder : SpriteEntity
    {
        const int NUM_SEGMENTS = 27;
        public const float LADDER_HEIGHT = NUM_SEGMENTS * 64;
        const float CAMERA_PADDING = 350;

        public static void Spawn(float xPos)
        {
            Game1.instance.entities.Add(new Ladder(xPos));
            Game1.boundary.Width = (int)((xPos - Game1.instance.camera.boundary.X) + CAMERA_PADDING);
        }

        public Ladder(float xPos)
            :base(new Vector2(xPos, Game1.boundary.Top), "ladder")
        {
        }

        public override void Render()
        {
            for (int i = 0; i < NUM_SEGMENTS; i++)
            {
                float fade = Game1.instance.fadeValue * Game1.instance.fadeValue;
                Game1.instance.spriteBatch.Draw(texture,
                    new Vector2(position.X - texture.Width / 2, position.Y - (i + 1) * texture.Height), new Color(fade, fade, fade, 1));
            }
        }



    }
}
