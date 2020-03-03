using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace lab04
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class lab04 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model model;
        
        Transform modelTransform;
        Transform cameraTransform;
        Camera camera;

        Model model2;
        Transform model2Transform;

        public lab04()
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
            model = Content.Load<Model>("Torus");
            modelTransform = new Transform();
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 5;
            camera = new Camera();
            camera.Transform = cameraTransform;

            model2 = Content.Load<Model>("Sphere");
            model2Transform = new Transform();
            model2Transform.LocalPosition = Vector3.Right * 4;
            model2Transform.Parent = modelTransform;

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



            InputManager.Update();
            Time.Update(gameTime);
            // TODO: Add your update logic here
            if (InputManager.IsKeyDown(Keys.W))
                cameraTransform.LocalPosition -= cameraTransform.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.S))
                cameraTransform.LocalPosition -= cameraTransform.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.A))
                cameraTransform.LocalPosition -= cameraTransform.Left * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.D))
                cameraTransform.LocalPosition -= cameraTransform.Right * Time.ElapsedGameTime;

            if (InputManager.IsKeyDown(Keys.Right))
            {
                modelTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
                System.Console.WriteLine("right");
            }
                

            if (InputManager.IsKeyDown(Keys.Left))
            {
                modelTransform.Rotate(Vector3.Up, -Time.ElapsedGameTime);
                System.Console.WriteLine("left");

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

            model.Draw(modelTransform.World, camera.View, camera.Projection);
            model2.Draw(model2Transform.World, camera.View, camera.Projection);
            base.Draw(gameTime);
        }
    }

}



