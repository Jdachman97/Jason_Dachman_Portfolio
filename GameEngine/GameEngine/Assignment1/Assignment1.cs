using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using CPI311.GameEngine;

namespace Assignment1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Assignment1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ProgressBar healthBackGround;
        ProgressBar health;
        ProgressBar distanceBackGround;
        ProgressBar distance;
        Sprite bomb;
        SpriteFont font;
        bool gameOver = false;
        bool gameWin = false;
        int forward = 0;
        AnimatedSprite player;
        Transform playerTransform;
        Random random = new Random();


        public Assignment1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            InputManager.Initialize();
            Time.Initialize();
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
            player = new AnimatedSprite (Content.Load<Texture2D>("Explorer"));
            player.Position = new Vector2(200, 200);

            healthBackGround = new ProgressBar(Content.Load<Texture2D>("Square"), 32.0f);
            healthBackGround.Position = new Vector2(215, 25);
            healthBackGround.Scale = new Vector2(5, 0.8f);
            
            health = new ProgressBar(Content.Load<Texture2D>("Square"), 32.0f);
            health.Position = new Vector2(215, 25);
            health.Scale = new Vector2(5, 0.8f);
            health.Color = Color.Red;

            distanceBackGround = new ProgressBar(Content.Load<Texture2D>("Square"), 32.0f);
            distanceBackGround.Position = new Vector2(540, 25);
            distanceBackGround.Scale = new Vector2(5, 0.8f);

            distance = new ProgressBar(Content.Load<Texture2D>("Square"), 0.0f);
            distance.Position = new Vector2(540, 25);
            distance.Scale = new Vector2(5, 0.8f);
            distance.Color = Color.Green;

            bomb = new Sprite(Content.Load<Texture2D>("clock_sprite"));
            bomb.Scale = new Vector2(0.1f, 0.1f);
            bomb.Source = new Rectangle(60, 60, 340, 340);
            bomb.Position = new Vector2(random.Next(60, 740), random.Next(60, 400));
            bomb.Color = Color.Yellow;

            font = Content.Load<SpriteFont>("Font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            
            

            InputManager.Update();
            Time.Update(gameTime);
            player.Update();
            healthBackGround.Update();
            health.Update();
            distanceBackGround.Update();
            distance.Update();
            // TODO: Add your update logic here
           
            health.Value -= (Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Up) && !gameOver && !gameWin)
            {
                distance.Value += Time.ElapsedGameTime * 0.65f;
                if (forward == 0)
                {
                    player.Position -= new Vector2(0, 1);
                }

                if(forward == 1)
                {
                    player.Position -= new Vector2(-1, 0);

                }

                if (forward == 2)
                {
                    player.Position -= new Vector2(0, -1);
                }

                if (forward == 3)
                {
                    player.Position -= new Vector2(1, 0);
                }
            }
            if (InputManager.IsKeyPressed(Keys.Right))
            {
                forward++;
               
            }
            if (InputManager.IsKeyPressed(Keys.Left))
            {
                forward--;
               
            }

            if(forward > 3)
            {
                forward = 0;
            }

            if(forward < 0)
            {
                forward = 3;
            }
            
            if((player.Position - bomb.Position).Length() < 55 ){
                health.Value += 3;
                bomb.Position = new Vector2(random.Next(60, 740), random.Next(60, 400));
            }


            if (forward == 0)
            {
                
                player.FrameY = 0;
            }

            if (forward == 1)
            {
                player.FrameY = 3;

            }

            if (forward == 2)
            {
                player.FrameY = 1;
            }

            if (forward == 3)
            {
                player.FrameY = 2;
            }
            base.Update(gameTime);

            if (health.Value > 32.0)
            {
                health.Value = 32;
            }

            if(health.Value <= 0)
            {
                health.Value = 0;
                gameOver = true;
            }

            if(distance.Value >= 32.0)
            {
                distance.Value = 32;
                health.Value = 32;
                gameWin = true;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            healthBackGround.Draw(spriteBatch);
            health.Draw(spriteBatch);
            distanceBackGround.Draw(spriteBatch);
            distance.Draw(spriteBatch);
            bomb.Draw(spriteBatch);
            spriteBatch.DrawString(font,
                "Time Remaining",
                new Vector2(10, 15),
                Color.White
                );
            spriteBatch.DrawString(font,
                "Distance Walked",
                new Vector2(335, 15),
                Color.White
                );
            if (gameOver)
            {
                spriteBatch.DrawString(font,
                "You Lose! :(",
                new Vector2(300, 180),
                Color.White
                );
            }
            if (gameWin)
            {
                spriteBatch.DrawString(font,
                "You Win! :)",
                new Vector2(300, 150),
                Color.White
                );
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
