using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Post_Scarcity
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public const int SCREEN_WIDTH = 1366;
        public const int SCREEN_HEIGHT = 768;
        const bool FULL_SCREEN = false;


        public static Rectangle boundary = new Rectangle(-200, -184, 10000000, 184 * 2);

        public static Game1 instance;

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public List<SpriteEntity> entities;
        Streak[] streaks = new Streak[Streak.NUM_STREAKS];
        public Camera camera;
        BasicEffect effect;
        SoundEffect streetNoise;
        SoundEffectInstance streetNoiseInstance;
        Background background;
        SpriteEntity sky;

        public UserControlledPerson userPerson;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            instance = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.IsFullScreen = FULL_SCREEN;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);
            streetNoise = Content.Load<SoundEffect>("street-noise");
            streetNoiseInstance = streetNoise.CreateInstance();
            streetNoiseInstance.IsLooped = true;
            streetNoiseInstance.Volume = 1.0f;
            streetNoiseInstance.Play();
            new DialogBox();
            newGame();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        void newGame()
        {
            entities = new List<SpriteEntity>();
            userPerson = new UserControlledPerson(Vector2.Zero);
            entities.Add(userPerson);
            entities.Add(new NPCAdult(700));
            camera = new Camera(new Vector2(0, 0));
            background = new Background();
            sky = new SpriteEntity(Vector2.Zero, "sky");
            Ladder.Spawn(100);
            Streak.InitializeWeights(true);
            for (int i = 0; i < streaks.Length; i++)
            {
                streaks[i] = new Streak();
            }
            Streak.InitializeWeights(false);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Input.Update(dt);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(dt);
            }

            foreach (Streak streak in streaks)
            {
                streak.Update(dt);
            }

            camera.Update(dt);
            background.Update(dt);
            sky.position = new Vector2(camera.position.X, (-2000) + 40);
            DialogBox.instance.Update(dt);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, camera.matrix);

            foreach (Streak streak in streaks)
            {
                streak.Render();
            }
            sky.Render();
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, camera.matrix);

            foreach (SpriteEntity entity in entities)
            {
                entity.Render();
            }
            background.Render();

            spriteBatch.End();

            DialogBox.instance.Render();

            base.Draw(gameTime);
        }
    }
}
