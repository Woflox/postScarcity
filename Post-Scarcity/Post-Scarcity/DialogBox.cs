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
    public class DialogBox
    {
        const float LETTER_TIME = 0.0425f;
        const float PUNCTUATION_TIME = 0.25f;
        const float FAST_QUOTIENT = 4;
        const int WIDTH = 700;
        const int PADDING = 40;
        const int MARGIN = 40;
        const int FONT_SCALE = 4;
        const float VOLUME = 0.125f;
        const float GAME_OVER_WAIT_TIME = 2.0f;

        public enum State
        {
            Closed,
            Reading,
            Finished
        }

        public static DialogBox instance;
        SpriteFont font;
        int charIndex;
        float timeOnLetter;
        string currentString;
        string textShown;
        Texture2D background;
        bool readingFast;
        bool soundThisFrame;
        float timeSinceFinalDialog = 0;

        public State state;
        SoundEffect letterSound;

        public DialogBox()
        {
            instance = this;
            font = Game1.instance.Content.Load<SpriteFont>("font");
            background = TextureLoader.LoadTexture("dialogbackground");
            letterSound = Game1.instance.Content.Load <SoundEffect>("dialog");
        }

        public void Update(float dt)
        {
            switch (state)
            {
                case State.Reading:
                    UpdateReading(dt);
                    break;
                case State.Finished:
                    UpdateFinished(dt);
                    break;
                case State.Closed:
                    break;
            }
        }

        public void UpdateReading(float dt)
        {
            if (Input.action)
            {
                readingFast = true;
            }
            char c = currentString[charIndex];
            float letterTime = (c == '.' || c == '?' || c == '!' || c == ',' || c == ';' || c == ':') 
                ? PUNCTUATION_TIME : LETTER_TIME;
            if (currentString[charIndex + 1] == '"')
            {
                letterTime = LETTER_TIME;
            }
            if (readingFast)
            {
                letterTime /= FAST_QUOTIENT;
            }
            timeOnLetter += dt;
            soundThisFrame = false;
            while (timeOnLetter > letterTime)
            {
                timeOnLetter -= letterTime;
                AddLetter();
                if (state == State.Finished)
                {
                    return;
                }
            }
        }

        public void UpdateFinished(float dt)
        {
            if (Game1.instance.userPerson.personTalking is NPCAdult)
            {
                if (Input.action)
                {
                    Close();
                }
            }
            else
            {
                if (Game1.instance.userPerson.endingConversationIndex == Game1.instance.userPerson.endingConversations.Length)
                {
                    timeSinceFinalDialog += dt;
                    if (timeSinceFinalDialog > GAME_OVER_WAIT_TIME)
                    {
                        Game1.instance.gameOver = true;
                    }
                }
                else
                {
                    if (Input.action)
                    {
                        ShowDialog(Game1.instance.userPerson.endingConversations[Game1.instance.userPerson.endingConversationIndex]);
                        Game1.instance.userPerson.endingConversationIndex++;
                    }    
                }
            }
        }

        void AddLetter()
        {
            charIndex += 1;

            textShown += currentString[charIndex];

            if (charIndex == currentString.Length - 1)
            {
                state = State.Finished;
            }

            char c = currentString[charIndex];
            if (c != ' ' && c != '\n')
            {
                if (!soundThisFrame)
                {
                    letterSound.Play(readingFast ? VOLUME * 0.75f : VOLUME, 0, 0);
                    soundThisFrame = true;
                }
            }
        }

        public void ShowDialog(string text)
        {
            currentString = AddNewLines(text);
            state = State.Reading;
            charIndex = -1;
            timeOnLetter = 0;
            textShown = "";
            readingFast = false;
            AddLetter();
            if (Game1.instance.userPerson.endingConversationIndex == Game1.instance.userPerson.endingConversations.Length - 1)
            {
                Game1.instance.StartOutroFade();
            }
        }

        string AddNewLines(string text)
        {
            string finalText = "";
            string[] words = text.Split(' ');
            int lineStart = 0;
            int lineLength = 0;

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (lineLength > 0 &&
                        font.MeasureString(text.Substring(lineStart, lineLength + word.Length)).X > WIDTH * FONT_SCALE)
                {
                    finalText += text.Substring(lineStart, lineLength) + "\n";
                    lineStart = lineStart + lineLength;
                    lineLength = 0;
                }
                lineLength += word.Length + 1;
            }
            finalText += text.Substring(lineStart);
            return finalText;
        }

        public void Close()
        {
            state = State.Closed;
            Game1.instance.userPerson.StopTalking();
        }

        public void Render()
        {
            if (state == State.Closed)
            {
                return;
            }
            Game1.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            Game1.instance.spriteBatch.Draw(background, new Rectangle(
                (Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth - (WIDTH + 2 * PADDING)) / 2, 
                MARGIN,
                WIDTH + PADDING * 2,
                (int)font.MeasureString(textShown).Y / FONT_SCALE + PADDING * 2), new Color(0, 0, 0, 0.5f));
            Game1.instance.spriteBatch.DrawString(font, textShown,
                new Vector2((Game1.instance.GraphicsDevice.PresentationParameters.BackBufferWidth - WIDTH) / 2,
                            MARGIN + PADDING), Color.White, 0, Vector2.Zero, 1f / FONT_SCALE, SpriteEffects.None, 0);
            Game1.instance.spriteBatch.End();
        }
    }
}
