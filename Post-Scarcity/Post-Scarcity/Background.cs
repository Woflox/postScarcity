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
    public class Background
    {
        const int DOTTED_LINE_DISTANCE = 196;
        const int LAMP_POST_DISTANCE = 1400;
        const int LAMP_POST_OFFSET = -600;
        const int DOTTED_LINE_Y = -25;
        const int SOLID_LINE_Y = -200;
        const int LAMP_POST_Y = -363;
        const float LAMP_POST_ANIMATION_RATE = 1 / 30f;
        static readonly Color DOTTED_LINE_COLOR = Color.Magenta;
        static readonly Color SOLID_LINE_COLOR = Color.White;

        Texture2D line;
        Texture2D lampPost1;
        Texture2D lampPost2;
        float animationProgress;
        int frame;

        public Background()
        {
            line = TextureLoader.LoadTexture("line");
            lampPost1 = TextureLoader.LoadTexture("lamppost1");
            lampPost2 = TextureLoader.LoadTexture("lamppost2");
        }

        public void Update(float dt)
        {
            animationProgress += dt / LAMP_POST_ANIMATION_RATE;
            animationProgress %= 1;

            frame = (int)(animationProgress * 2);
            if (frame == 2)
            {
                frame -= 1;
            }
        }

        public void Render()
        {
            int dottedXStart = (int)Math.Ceiling((double)(Game1.instance.camera.boundary.Left - line.Width) / DOTTED_LINE_DISTANCE) * DOTTED_LINE_DISTANCE;

            for (int x = dottedXStart; x < Game1.instance.camera.boundary.Right; x += DOTTED_LINE_DISTANCE)
            {
                Game1.instance.spriteBatch.Draw(line,
                    new Vector2(x, DOTTED_LINE_Y),
                    DOTTED_LINE_COLOR);
            }

            Game1.instance.spriteBatch.Draw(line,
                new Rectangle((int)Game1.instance.camera.boundary.Left - 2,
                                SOLID_LINE_Y,
                                Game1.instance.camera.boundary.Width + 4,
                                line.Height),
                SOLID_LINE_COLOR);

            foreach (SpriteEntity entity in Game1.instance.entities)
            {
                if (entity is NPCChild)
                {
                    return;
                }
            }

            int lampPostXStart = (int)Math.Ceiling((double)(Game1.instance.camera.boundary.Left - lampPost1.Width) / LAMP_POST_DISTANCE) * LAMP_POST_DISTANCE + LAMP_POST_OFFSET;

            for (int x = lampPostXStart; x < Game1.instance.camera.boundary.Right; x += LAMP_POST_DISTANCE)
            {
                Game1.instance.spriteBatch.Draw(frame == 0 ? lampPost1 : lampPost2,
                    new Vector2(x, LAMP_POST_Y),
                    Color.White);
            }

        }
    }
}