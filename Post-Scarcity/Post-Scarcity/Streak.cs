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
    public class Streak
    {
        public const int NUM_STREAKS = 3000;
        const float SPEED = 5000;
        const float MAX_Y = -240;
        const float MIN_Y = -2000;
        const float MIN_SCALE = 0.05f;
        const float MIN_SPEED = 0.001f;
        const float MIN_P = 0.01f;

        enum Direction
        {
            Left,
            Right
        }

        static Random rand = new Random();
        static List<Color> colors = new List<Color> { Color.Magenta, Color.Blue, Color.Magenta, Color.Blue, Color.Yellow };

        Vector2 position;
        Color color;
        float scale;
        float speed;
        Direction direction;
        Texture2D texture;

        public Streak()
        {
            Init(true);
            texture = TextureLoader.LoadTexture("streak");
        }
        
        static float[] weights;

        public static void InitializeWeights(bool startingWeights)
        {
            weights = new float[(int)(MAX_Y - MIN_Y)];

            float weightTotal = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                float ratio = ((float)i) / weights.Length;
                weights[i] = 1 / (MIN_P + ratio * (1 - MIN_P));
                weights[i] *= 1 - ratio;
                if (startingWeights)
                {
                    weights[i] *= weights[i];
                }
                weightTotal += weights[i];
            }

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] /= weightTotal;
            }
        }

        public void Init(bool first)
        {
            float r = (float)rand.NextDouble();
            float cumulative = 0;
            int index = 0;
            for (index = 0; index < weights.Length; index++)
            {
                cumulative += weights[index];
                if (cumulative >= r)
                {
                    break;
                }
            }

            float y = ((float)index) / weights.Length;

            position.Y = MIN_Y + y * (MAX_Y - MIN_Y);
            direction = rand.Next(2) == 0 ? Direction.Left : Direction.Right;
            color = colors[rand.Next(colors.Count)];
            scale = MIN_SCALE + y * (1 - MIN_SCALE);
            scale *= 2;
            speed = (MIN_SPEED + y * (1 - MIN_SPEED)) * SPEED;

            if (first)
            {
                position.X = Game1.instance.camera.boundary.Left
                    + (float)rand.NextDouble() * Game1.instance.camera.boundary.Width;
            }
            else
            {
                if (direction == Direction.Left)
                {
                    position.X = Game1.instance.camera.boundary.Right + texture.Width * scale / 2;
                }
                else
                {
                    position.X = Game1.instance.camera.boundary.Left - texture.Width * scale / 2;
                }
            }

            if (!first)
            {
                if (position.Y > Game1.instance.camera.boundary.Top)
                {
                    float volume = 0.75f * (float)Math.Pow(1 - Game1.instance.soundFade, 5);
                    Game1.instance.carComing.Play(volume, 0, direction == Direction.Left ? 1 : -1);
                    Game1.instance.carLeaving.Play(volume, 0, direction == Direction.Left ? -1 : 1); 
                }

            }
        }

        public void Update(float dt)
        {
            position.X += speed * dt * (direction == Direction.Left ? -1 : 1);

            position.X += ((SPEED - speed) / SPEED)  * (Game1.instance.camera.position.X - Game1.instance.camera.lastPos.X);

            if ((position.X < Game1.instance.camera.boundary.Left - texture.Width * scale)
             || (position.X > Game1.instance.camera.boundary.Right + texture.Width * scale))
            {
                Init(false);
            }
        }

        public void Render()
        {
            if (position.Y + texture.Height > Game1.instance.camera.boundary.Top
             && position.Y < Game1.instance.camera.boundary.Bottom)
            {
                Game1.instance.spriteBatch.Draw(texture,
                    new Vector2(position.X - texture.Width * scale / 2, position.Y),
                    null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }
    }
}
