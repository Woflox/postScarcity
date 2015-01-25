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
    public class Person : AnimatedSpriteEntity
    {
        public enum State
        {
            Normal,
            Talking,
            Climbing
        }

        const float COLOR_CHANGE_RATE = 1 / 28f;
        const float MOVE_SPEED = 128;
        const float ISOMETRIC_Y_COEFFICIENT = 0.75f;
        const float FLIP_HYSTERISIS = 0.1f;
        const float WALK_ANIMATION_RATE = 0.35f;
        const float CLIMB_ANIMATION_RATE = 0.35f;

        float t = 0;
        protected Vector2 moveDir;
        public State state;
        protected Animation animation;

        List<Color> colors;

        public Person(Vector2 pos, string textureName, List<Color> colors)
            : base(pos, textureName, 58)
        {
            this.colors = colors;
            animations.Add(new Animation("Idle", 0, 1));
            animations.Add(new Animation("Walking", 1, 2));
            animations.Add(new Animation("Climbing", 3, 2));
        }

        public override void Update(float dt)
        {
            t += dt;

            color = colors[((int)(t / COLOR_CHANGE_RATE)) % colors.Count];

            switch (state)
            {
                case State.Normal:
                    if (moveDir != Vector2.Zero)
                    {
                        position.X += moveDir.X * dt * MOVE_SPEED;
                        position.Y += moveDir.Y * ISOMETRIC_Y_COEFFICIENT * dt * MOVE_SPEED;
                    }
                    break;
                case State.Talking:
                    moveDir = Vector2.Zero;
                    break;
                case State.Climbing:
                    position.Y += moveDir.Y;
                    break;
            }

            if (state != State.Climbing)
            {
                if (moveDir == Vector2.Zero)
                {
                    SetAnimation("Idle");
                }
                else
                {
                    SetAnimation("Walking");
                    if (flipped && moveDir.X > FLIP_HYSTERISIS)
                    {
                        flipped = false;
                    }
                    else if (!flipped && moveDir.X < -FLIP_HYSTERISIS)
                    {
                        flipped = true;
                    }
                    animationRate = WALK_ANIMATION_RATE * moveDir.Length();
                }
            }
            else
            {
                SetAnimation("Climbing");
                animationRate = CLIMB_ANIMATION_RATE * moveDir.Y;
            }

            base.Update(dt);
        }

    }
}
