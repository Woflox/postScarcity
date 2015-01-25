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
    public static class Input
    {
        static GamePadState gamePadState;
        static GamePadState lastGamePadState;
        static KeyboardState keyboardState;
        static KeyboardState lastKeyboardState;

        public static Vector2 moveDir;
        public static bool action;

        const Keys ACTION_KEY = Keys.Space;
        const Buttons ACTION_BUTTON = Buttons.A;

        public static void Update(float dt)
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();

            moveDir = new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);

            if (moveDir == Vector2.Zero)
            {
                if (gamePadState.IsButtonDown(Buttons.DPadLeft)
                  || keyboardState.IsKeyDown(Keys.Left))
                {
                    moveDir.X -= 1;
                }
                if (gamePadState.IsButtonDown(Buttons.DPadRight)
                    || keyboardState.IsKeyDown(Keys.Right))
                {
                    moveDir.X += 1;
                }
                if (gamePadState.IsButtonDown(Buttons.DPadUp)
                    || keyboardState.IsKeyDown(Keys.Up))
                {
                    moveDir.Y -= 1;
                }
                if (gamePadState.IsButtonDown(Buttons.DPadDown)
                    || keyboardState.IsKeyDown(Keys.Down))
                {
                    moveDir.Y += 1;
                }
                if (moveDir != Vector2.Zero)
                {
                    moveDir.Normalize();
                }
            }

            if ((gamePadState.IsButtonDown(ACTION_BUTTON) && !lastGamePadState.IsButtonDown(ACTION_BUTTON))
              || (keyboardState.IsKeyDown(ACTION_KEY) && !lastKeyboardState.IsKeyDown(ACTION_KEY)))
            {
                action = true;
            }
            else
            {
                action = false;
            }

            lastGamePadState = gamePadState;
            lastKeyboardState = keyboardState;
        }

    }
}
