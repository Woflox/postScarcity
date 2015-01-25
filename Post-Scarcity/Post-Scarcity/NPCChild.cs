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
    public class NPCChild : Person
    {
        public NPCChild(float xPos)
            : base(new Vector2(xPos, Game1.boundary.Top + 20), "girl", new List<Color> { Color.LightBlue, Color.DarkBlue, new Color(0, 128, 255), Color.Blue, Color.LightBlue, Color.DarkBlue})
        {
        }

        public override void Update(float dt)
        {
            if (state == State.Normal)
            {
                flipped = position.X > Game1.instance.userPerson.position.X;
            }

            base.Update(dt);
        }
    }
}
