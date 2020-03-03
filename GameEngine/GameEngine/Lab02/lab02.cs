using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using CPI311.GameEngine;

namespace Lab02
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class lab02 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite sprite;
        float phase = 0.0f;
        float phaseSpeed = 1.0f;
        int radius = 50;
        //KeyboardState prevState;

        public lab02()
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
            // prevState = Keyboard.GetState();


            base.Initialize();
         
        

    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
        {
          
            spriteBatch = new SpriteBatch(GraphicsDevice);

            sprite = new Sprite(Content.Load<Texture2D>("Square"));

            InputManager.Initialize();
            Time.Initialize();

            sprite.Position = new Vector2(200, 200);

            // TODO: use this.Content to load your game content here
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

            KeyboardState currentState = Keyboard.GetState();
            /*  if (currentState.IsKeyDown(Keys.Left) &&
                  prevState.IsKeyUp(Keys.Left))
                  sprite.Position += Vector2.UnitX * -5;
              if (currentState.IsKeyDown(Keys.Right) &&
                  prevState.IsKeyUp(Keys.Right))
                  sprite.Position += Vector2.UnitX * 5;
              if (currentState.IsKeyDown(Keys.Up) &&
                  prevState.IsKeyUp(Keys.Up))
                  sprite.Position += Vector2.UnitY * -5;
              if (currentState.IsKeyDown(Keys.Down) &&
                  prevState.IsKeyUp(Keys.Down))
                  sprite.Position += Vector2.UnitY * 5;


              prevState = currentState;
  */
            InputManager.Update();
            Time.Update(gameTime);
            phase += Time.ElapsedGameTime;
            if (InputManager.IsKeyPressed(Keys.Right))
            {
                radius += 25;

            }

            if (InputManager.IsKeyPressed(Keys.Left))
            {
                radius -= 25;

            }

            if (InputManager.IsKeyPressed(Keys.Up))
            {
                phaseSpeed += 0.25f;

            }

            if (InputManager.IsKeyPressed(Keys.Down))
            {
                phaseSpeed -= 0.25f;

            }

            //if (InputManager.IsKeyDown(Keys.Space))
                sprite.Rotation = 1.0f * (phase * phaseSpeed);

            sprite.Position = new Vector2(400, 240) + new Vector2(
                (float)(radius * Math.Cos(phase * phaseSpeed)),
                (float)(radius * Math.Sin(phase * phaseSpeed)));


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            sprite.Draw(spriteBatch);
            spriteBatch.End();
            

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
