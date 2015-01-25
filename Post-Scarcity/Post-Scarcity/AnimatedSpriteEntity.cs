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
    public class Animation
    {
        public string name;
        public int startFrame;
        public int numFrames;
        public float progress = 0;

        public Animation(string name, int startFrame, int numFrames)
        {
            this.name = name;
            this.startFrame = startFrame;
            this.numFrames = numFrames;
        }
    }

    public class AnimatedSpriteEntity : SpriteEntity
    {
        int spriteWidth;
        public bool flipped;
        protected List<Animation> animations = new List<Animation>();
        Animation animation;
        int frame;
        protected float animationRate;

        public AnimatedSpriteEntity(Vector2 pos, string textureName, int spriteWidth)
            : base(pos, textureName)
        {
            this.spriteWidth = spriteWidth;
        }

        public override void Update(float dt)
        {
            if (animationRate > 0)
            {
                animation.progress += dt / animationRate;
                animation.progress %= 1;

                frame = animation.startFrame + (int)(animation.progress * animation.numFrames);
                if (frame == animation.startFrame + animation.numFrames)
                {
                    frame -= 1;
                }
            }

            base.Update(dt);
        }

        public void SetAnimation(string animationName)
        {
            foreach (Animation anim in animations)
            {
                if (anim.name == animationName)
                {
                    animation = anim;
                    return;
                }
            }
        }

        public override void Render()
        {
            Rectangle srcRect;
            srcRect = new Rectangle(frame * spriteWidth, 0, spriteWidth, texture.Height);

            Game1.instance.spriteBatch.Draw(texture,
                                            new Vector2(position.X - spriteWidth / 2, position.Y - texture.Height),
                                            new Rectangle(frame*spriteWidth, 0, spriteWidth, texture.Height),
                                            color, 0, Vector2.Zero, 1,
                                            flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                                            Math.Max(0.0001f,(position.Y - Game1.boundary.Y) / Game1.boundary.Height));
        }
    }
}
