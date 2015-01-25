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
    public class NPCAdult : Person
    {
        static Random rand = new Random();
        const float RELATIVE_WALK_SPEED = 0.75f;
        const float DECISION_CHANCE = 0.001f;

        enum AIState
        {
            Idle,
            Walking
        }

        AIState aiState;
        public bool neverMove = false;
        public bool alreadyTalked = false;

        public NPCAdult(float x)
            : base(new Vector2(x,0), "man", new List<Color> { Color.Blue, Color.Black, new Color(0, 128, 255), Color.DarkBlue, Color.Blue, Color.Black })
        {
            position.Y = Game1.boundary.Y + (float)rand.NextDouble() * Game1.boundary.Height;
            if (rand.Next(2) == 0)
            {
                aiState = AIState.Idle;
            }
            else
            {
                aiState = AIState.Walking;
            }

            if (position.X < 750)
            {
                neverMove = true;
            }

            MakeDecision();
        }

        void MakeDecision()
        {
            if (neverMove)
            {
                aiState = AIState.Idle;
                flipped = false;
                return;
            }

            if (aiState == AIState.Walking)
            {
                aiState = AIState.Idle;
                return;
            }
            aiState = AIState.Walking;
            flipped = rand.Next(2) == 0;
        }

        public override void Update(float dt)
        {
            if (rand.NextDouble() < DECISION_CHANCE)
            {
                MakeDecision();
            }
            switch (aiState)
            {
                case AIState.Walking:
                    moveDir = new Vector2(flipped ? -RELATIVE_WALK_SPEED : RELATIVE_WALK_SPEED, 0);
                    break;
                case AIState.Idle:
                    moveDir = Vector2.Zero;
                    break;
            }
            base.Update(dt);
        }
    }
}
