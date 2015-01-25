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
        const int RENDER_DISTANCE = 1500;
        const int DOTTED_LINE_Y = -25;
        const int SOLID_LINE_Y = -200;
        const int LAMP_POST_Y = -365;
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
            int dottedXStart = ((int)(Game1.instance.userPerson.position.X / DOTTED_LINE_DISTANCE)) * DOTTED_LINE_DISTANCE;
            int dottedRenderCount = (RENDER_DISTANCE * 2) / DOTTED_LINE_DISTANCE;
            dottedXStart -= (dottedRenderCount / 2) * DOTTED_LINE_DISTANCE;

            for (int i = 0; i < dottedRenderCount; i++)
            {
                Game1.instance.spriteBatch.Draw(line,
                    new Vector2(dottedXStart + i * DOTTED_LINE_DISTANCE, DOTTED_LINE_Y),
                    DOTTED_LINE_COLOR);
            }

            Game1.instance.spriteBatch.Draw(line,
                new Rectangle((int)Game1.instance.userPerson.position.X - RENDER_DISTANCE,
                                SOLID_LINE_Y,
                                RENDER_DISTANCE * 2,
                                line.Height),
                SOLID_LINE_COLOR);

            int lampPostXStart = ((int)(Game1.instance.userPerson.position.X / LAMP_POST_DISTANCE)) * LAMP_POST_DISTANCE + LAMP_POST_OFFSET;
            int lampPostRenderCount = (RENDER_DISTANCE * 2) / LAMP_POST_DISTANCE;
            lampPostXStart -= (lampPostRenderCount / 2) * LAMP_POST_DISTANCE;

            for (int i = 0; i < 8; i++)
            {
                Game1.instance.spriteBatch.Draw(frame == 0 ? lampPost1 : lampPost2,
                    new Vector2(lampPostXStart + i * LAMP_POST_DISTANCE, LAMP_POST_Y),
                    Color.White);
            }

        }
    }
}
