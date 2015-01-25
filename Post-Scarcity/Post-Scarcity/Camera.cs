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
    public class Camera
    {
        Vector2 position;
        public Matrix matrix;

        const float SMOOTH_COEFFICIENT = 0.985f;
        static readonly Vector2 OFFSET = new Vector2(400, -200);

        public Camera(Vector2 pos)
        {
            position = pos + OFFSET;
        }

        public void Update(float dt)
        {
            foreach (SpriteEntity entity in Game1.instance.entities)
            {
                UserControlledPerson target = entity as UserControlledPerson;
                if (target != null)
                {
                    Vector2 targetPos = new Vector2(target.position.X, 0) + OFFSET;
                    if (target.flipped)
                    {
                        targetPos.X -= OFFSET.X * 0.7f;
                    }
                    position = position * SMOOTH_COEFFICIENT + targetPos * (1 - SMOOTH_COEFFICIENT);
                    position.X = Math.Max(Game1.boundary.X + Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth/2, position.X);
                    position.X = Math.Min(Game1.boundary.X + Game1.boundary.Width - Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth / 2, position.X);

                }
            }

            matrix = Matrix.CreateTranslation(
                new Vector3(-position.X + Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth/2,                                
                            -position.Y + Game1.instance.GraphicsDevice.PresentationParameters.BackBufferHeight / 2, 
                            0));
        }
    }
}
