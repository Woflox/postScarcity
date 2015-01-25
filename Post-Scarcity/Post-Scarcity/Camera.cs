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
        const float MIN_Y = -2400;

        public Vector2 position;
        public Matrix matrix;

        const float SMOOTH_COEFFICIENT = 0.985f;
        static readonly Vector2 OFFSET = new Vector2(400, -200);
        public Rectangle boundary;
        public Vector2 lastPos;

        public Camera(Vector2 pos)
        {
            position = pos + OFFSET;
            lastPos = position;
            Update(0);
        }
       // float yoffset = 0;
        public void Update(float dt)
        {
            lastPos = position;
            //yoffset -= 2;
            Vector2 targetPos = new Vector2(Game1.instance.userPerson.position.X, 0) + OFFSET;
            if (Game1.instance.userPerson.state == Person.State.Climbing ||
                 Game1.instance.userPerson.state == Person.State.Looking)
            {
                targetPos.Y += Game1.instance.userPerson.position.Y;
            }

            if (Game1.instance.userPerson.flipped)
            {
                targetPos.X -= OFFSET.X * 0.7f;
            }
            targetPos.X = Math.Max(Game1.boundary.X + Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth / 2, targetPos.X);
            targetPos.X = Math.Min(Game1.boundary.X + Game1.boundary.Width - Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth / 2, targetPos.X);
            targetPos.Y = Math.Max(targetPos.Y, MIN_Y + Game1.instance.GraphicsDevice.PresentationParameters.BackBufferHeight / 2);

            position = position * SMOOTH_COEFFICIENT + targetPos * (1 - SMOOTH_COEFFICIENT);

            matrix = Matrix.CreateTranslation(
                new Vector3(-position.X + Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth/2,                                
                            -position.Y + Game1.instance.GraphicsDevice.PresentationParameters.BackBufferHeight / 2, 
                            0));

            boundary = new Rectangle((int)position.X - Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth / 2,
                                (int)position.Y - Game1.instance.GraphicsDevice.PresentationParameters.BackBufferHeight / 2,
                                Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth,
                                Game1.instance.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }
    }
}
