using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace Lab03
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab03 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model Model;
        Matrix world, view, projection;
        Vector3 cameraPosition = new Vector3(0, 0, 5);
        Vector3 torusPosition = new Vector3(0, 0, 0);
        Vector3 torusRotation = new Vector3(0, 0, 0);
        Vector3 torusScale = new Vector3(1, 1, 1);
        float nearPlane = 0.1f;
        bool isPerspective = true;
        bool isNotReversed = true;
        SpriteFont font;

        public Lab03()
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
            Model = Content.Load<Model>("Torus");
            font = Content.Load<SpriteFont>("Font");
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

            // TODO: Add your update logic her

            InputManager.Update();
            Time.Update(gameTime);

            if (InputManager.IsKeyDown(Keys.Left))
            {
                torusPosition.X -= 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.Delete))
            {
                torusRotation.X -= 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.Insert))
            {
                torusRotation.X += 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.PageUp))
            {
                torusRotation.Y -= 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.PageDown))
            {
                torusRotation.Y += 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.Home))
            {
                torusRotation.Z -= 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.End))
            {
                torusRotation.Z += 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.Right))
            {
                torusPosition.X += 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.Up) && !InputManager.IsKeyDown(Keys.LeftShift))
            {
                torusPosition.Y += 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.Down) && !InputManager.IsKeyDown(Keys.LeftShift))
            {
                torusPosition.Y -= 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.Up) && InputManager.IsKeyDown(Keys.LeftShift))
            {
                torusScale *= 1.01f;
            }

            if (InputManager.IsKeyDown(Keys.Down) && InputManager.IsKeyDown(Keys.LeftShift))
            {
                torusScale *= 0.99f;
            }

            if (InputManager.IsKeyDown(Keys.W) && !InputManager.IsKeyDown(Keys.LeftShift))
            {
                cameraPosition -= Vector3.Up * Time.ElapsedGameTime * 5;
            }

            if (InputManager.IsKeyDown(Keys.S) && !InputManager.IsKeyDown(Keys.LeftShift))
            {
                cameraPosition -= Vector3.Down * Time.ElapsedGameTime * 5;
            }

            if (InputManager.IsKeyDown(Keys.A))
            {
                cameraPosition -= Vector3.Left * Time.ElapsedGameTime * 5;
            }

            if (InputManager.IsKeyDown(Keys.D))
            {
                cameraPosition -= Vector3.Right * Time.ElapsedGameTime * 5;
            }

            if (InputManager.IsKeyDown(Keys.W) && InputManager.IsKeyDown(Keys.LeftShift))
            {
                nearPlane *= 1.03f;
            }

            if (InputManager.IsKeyDown(Keys.S) && InputManager.IsKeyDown(Keys.LeftShift))
            {
                nearPlane *= 0.97f;
            }

            if (InputManager.IsKeyPressed(Keys.Tab))
            {
                isPerspective = !isPerspective;
            }

            if (InputManager.IsKeyPressed(Keys.Space))
            {
                isNotReversed = !isNotReversed;
            }

            view = Matrix.CreateLookAt(
                cameraPosition,
                cameraPosition + Vector3.Forward,            //Vector3.Zero,
                Vector3.Up
                );

            if (isPerspective)
            {
                projection = Matrix.CreatePerspectiveFieldOfView(

                   MathHelper.PiOver2,
                   1.33f,
                   nearPlane,
                   300f
                   );
                   
               /* projection = Matrix.CreatePerspectiveOffCenter(
                    1, 1, 1, 1, 10.0f, 100f
                    );*/

            }
            else
            {
                projection = Matrix.CreateOrthographic(
                      1, 1, 0.1f, 100f
                    );
            }

            if (isNotReversed)
            {
                world = Matrix.CreateScale(torusScale) *
                Matrix.CreateFromYawPitchRoll(torusRotation.X, torusRotation.Y, torusRotation.Z) *
                Matrix.CreateTranslation(torusPosition);
            }
            else
            {
               world = Matrix.CreateTranslation(torusPosition) *
                Matrix.CreateFromYawPitchRoll(torusRotation.X, torusRotation.Y, torusRotation.Z) *
                Matrix.CreateScale(torusScale);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            
            // TODO: Add your drawing code here
            Model.Draw(world, view, projection);
            

            spriteBatch.Begin();
            spriteBatch.DrawString(font,
                "CameraPos: " + cameraPosition,
                new Vector2(10, 10),
                Color.White
                );
            spriteBatch.DrawString(font,
                "Use Arrow Keys to Move Object",
                new Vector2(10, 30),
                Color.White
                );
            spriteBatch.DrawString(font,
                "Use WASD to Move Camera",
                new Vector2(10, 50),
                Color.White
                );
            spriteBatch.DrawString(font,
                "Use Insert/Delete for Yaw",
                new Vector2(10, 70),
                Color.White
                );
            spriteBatch.DrawString(font,
                "Use Home/End for Pitch",
                new Vector2(10, 90),
                Color.White
                );
            spriteBatch.DrawString(font,
                "Use PageUp/PageDown for Roll",
                new Vector2(10, 110),
                Color.White
                );
            spriteBatch.DrawString(font,
                "Use shift + up or down arrows for Scale",
                new Vector2(10, 130),
                Color.White
                );
            spriteBatch.DrawString(font,
                "Use spaceBar to reverse drawing matrix",
                new Vector2(10, 150),
                Color.White
                );
            spriteBatch.DrawString(font,
                "Use Tab to toggle between orthographic/perspective mode",
                new Vector2(10, 170),
                Color.White
                );
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
