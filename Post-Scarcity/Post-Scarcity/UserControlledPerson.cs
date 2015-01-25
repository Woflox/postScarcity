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
    public class UserControlledPerson : Person
    {
        static Random rand = new Random();

        const float MIN_SPAWN_X = 400;
        const float SPAWN_EVERY = 400;
        const float SPAWN_MIN_DISTANCE = 1200;
        const float SPAWN_RANGE = 1000;
        const float CONVERSATION_DISTANCE = 30;
        const float LADDER_CLIMB_DISTANCE = 16;

        float spawnX = MIN_SPAWN_X;
        Person personTalking = null;

        int conversationIndex = 0;

        public bool conversationsExhausted = false;
        public bool reachedTop = false;


        string[] regularConversions =
        {
            "I don't have to worry about money any more.\n\nBut I feel a bit empty.",
            "The world moves so fast now. I can't keep up.",
            "They're better than us. Smarter and more efficient.\n\nI feel so inferior.",
            "We created them but I don't think that matters. Nobody owns anything any more.",
            "We're obsolete. There's nothing you can do about it.",
            "I've heard that humans are happier than ever before. Are you happy?",
            "We used to work for a living. We worked towards our future. Built the walls that surrounded us.\n\nWhat do we do now? I've been walking these streets for days and I haven't found anything.",
            "I remember working in a factory. My body was sore and I was always tired, but it was a big part of my life.\n\nNow I can do whatever I want, but I don't know what to do.",
            "My daughter's view of the world is so different from mine. She doesn't know what it was like before.\n\nI can't relate to her.",
            "I feel like the moments in my life aren't connected together in the same way as before.\n\nWithout progression, things don't feel sequential. Like yesterday and last year are the same thing and I couldn't tell you which is which.",
            "I try to create. To express things that are uniquely human, uniquely me. But I feel as if I'm speaking into a void, into empty space.\n\nWhat can I express that's new if the robots know us better than we know ourselves?",
            "What drive is there to achieve when we are forever in their shadow? Our contributions are infinitesimal.",
            "Yesterday I made a painting of a representation of what I remembered the world being like.\n\nPart of it featured a giant man holding the Earth on his back.\n\nI threw it off my balcony."
        };

        int endingConversationIndex = 0;
        string[] endingConversations =
        {
            "The world is big. Unfathomably so.\n\nEven before this upheaval we were minuscule, each human one amongst billions with no way of truly grasping our own insignificance.",
            "This is just another step in our history. This is where we abandon our pride and our arrogance. This is where we create for the sake of creating; live for the sake of living. This is where we open our eyes and see the world around us.",
            "What do we do now? The answer is simple:",
            "We live."
        };

        public UserControlledPerson(Vector2 pos)
            : base(pos, "man", new List<Color> { Color.Magenta, Color.Yellow, Color.Blue, Color.White })
        {
        }

        public override void Update(float dt)
        {
            moveDir = Input.moveDir;
            base.Update(dt);

            if (state == State.Normal)
            {
                position.X = Math.Max(position.X, Game1.boundary.Left);
                position.X = Math.Min(position.X, Game1.boundary.Right);
                position.Y = Math.Max(position.Y, Game1.boundary.Top);
                position.Y = Math.Min(position.Y, Game1.boundary.Bottom);

                foreach (SpriteEntity entity in Game1.instance.entities)
                {
                    NPCAdult npc = entity as NPCAdult;
                    if (npc != null)
                    {
                        if (state == State.Normal
                         && !npc.hasTalked
                         && conversationIndex < regularConversions.Length
                         && Math.Abs(position.X - npc.position.X) < CONVERSATION_DISTANCE
                         && Vector2.Distance(position, npc.position) < CONVERSATION_DISTANCE)
                        {
                            TalkTo(npc);
                        }
                        continue;
                    }
                    Ladder ladder = entity as Ladder;
                    if (ladder != null)
                    {
                        if (state == State.Normal
                            && moveDir.Y < 0
                            && Vector2.Distance(position, ladder.position) < LADDER_CLIMB_DISTANCE)
                        {
                            position = ladder.position;
                            state = State.Climbing;
                        }
                    }
                }

                if (position.X > spawnX && !reachedTop)
                {
                    spawnX += SPAWN_EVERY;
                    Game1.instance.entities.Add(new NPCAdult(position.X + SPAWN_MIN_DISTANCE + (float)rand.NextDouble() * SPAWN_RANGE));
                }
            }
            else if (state == State.Climbing || state == State.Looking)
            {
                float progress = (Game1.boundary.Top - position.Y) / Ladder.LADDER_HEIGHT;
                //TODO: music fade in
            }
            if (state == State.Looking)
            {
                if (!reachedTop)
                {
                    reachedTop = true;

                }
            }
        }

        void TalkTo(Person person)
        {
            person.hasTalked = true;
            person.state = State.Talking;
            state = State.Talking;
            personTalking = person;
            person.flipped = person.position.X > position.X;
            DialogBox.instance.ShowDialog(regularConversions[conversationIndex]);
            conversationIndex += 1;
        }

        public void StopTalking()
        {
            if (true)//conversationIndex == regularConversions.Length && !conversationsExhausted)
            {
                foreach (SpriteEntity entity in Game1.instance.entities)
                {
                    NPCAdult npc = entity as NPCAdult;
                    if (npc != null)
                    {
                        npc.MakeInactive();
                    }
                }
                conversationsExhausted = true;
                Ladder.Spawn(position.X + SPAWN_MIN_DISTANCE);
            }
            else
            {
                NPCAdult npc = personTalking as NPCAdult;
                if (npc != null)
                {
                    npc.MakeInactive();
                }
            }

            state = State.Normal;
            personTalking = null;

        }

    }
}
