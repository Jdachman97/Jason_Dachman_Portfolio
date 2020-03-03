using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using CPI311.GameEngine.Rendering;
using System;
using System.Collections.Generic;

namespace Assignment5
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Assignment5 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TerrainRenderer terrain;
        Camera camera;
        Camera topDownCamera;
        List<Camera> cameras;
        Effect effect;
        Player player;
        Agent agent;
        Agent agent1;
        Agent agent2;
        Light light;
        SpriteFont font;
        int hits;
        Random random;

        public Assignment5()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
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

            Time.Initialize();
            ScreenManager.Initialize(graphics);
            ScreenManager.Setup(true, 1920, 1080);
            InputManager.Initialize();
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
            font = Content.Load<SpriteFont>("File");
            terrain = new TerrainRenderer(Content.Load<Texture2D>("MazeH2"), Vector2.One * 100, Vector2.One * 200);
            terrain.NormalMap = Content.Load<Texture2D>("MazeN2");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 5, 1);

            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["Shininess"].SetValue(20.0f);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);





            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Up * 60;
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            camera.Size = new Vector2(0.5f, 1f);
            camera.AspectRatio = camera.Viewport.AspectRatio;

            topDownCamera = new Camera();
            topDownCamera.Transform = new Transform();
            topDownCamera.Transform.LocalPosition = Vector3.Up * 10;
            topDownCamera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            topDownCamera.Position = new Vector2(0.5f, 0f);
            topDownCamera.Size = new Vector2(0.5f, 1f);
           
            cameras = new List<Camera>();
            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;

            random = new Random();

            cameras.Add(topDownCamera);
            cameras.Add(camera);

            player = new Player(terrain, Content, camera, GraphicsDevice, light);

            agent = new Agent(terrain, Content, camera, GraphicsDevice, light, random);
            agent1 = new Agent(terrain, Content, camera, GraphicsDevice, light, random);
            agent2 = new Agent(terrain, Content, camera, GraphicsDevice, light, random);

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
            Time.Update(gameTime);
            InputManager.Update();
            player.Update();
            agent.Update();
            agent1.Update();
            agent2.Update();

          /*  if (InputManager.IsKeyDown(Keys.W)) // move forward
                camera.Transform.LocalPosition += camera.Transform.Forward * Time.ElapsedGameTime * 6;

            if (InputManager.IsKeyDown(Keys.S)) // move forward
                camera.Transform.LocalPosition += camera.Transform.Backward * Time.ElapsedGameTime * 6;

            if (InputManager.IsKeyDown(Keys.A)) // move forward
                camera.Transform.Rotate(camera.Transform.Up, Time.ElapsedGameTime);

            if (InputManager.IsKeyDown(Keys.D)) // move forward
                camera.Transform.Rotate(camera.Transform.Down, Time.ElapsedGameTime);
                */

            if (agent.CheckCollision(player))
            {
                hits++;
            }
            if (agent1.CheckCollision(player))
            {
                hits++;
            }
            if (agent2.CheckCollision(player))
            {
                hits++;
            }



            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            foreach (Camera camera in cameras)
            {
                GraphicsDevice.Viewport = cameras[0].Viewport;
                // draw items using cameras[0]
                GraphicsDevice.Viewport = cameras[1].Viewport;
                Matrix view = camera.View;
                Matrix projection = camera.Projection;
                // draw items using cameras[1]

                GraphicsDevice.Clear(Color.CornflowerBlue);
                GraphicsDevice.DepthStencilState = new DepthStencilState();

                // TODO: Add your drawing code here

                effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);
                effect.Parameters["World"].SetValue(terrain.Transform.World);
                effect.Parameters["View"].SetValue(camera.View);
                effect.Parameters["Projection"].SetValue(camera.Projection);
                effect.Parameters["LightPosition"].SetValue(camera.Transform.Position + Vector3.Up * 10);
                effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);


                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    terrain.Draw();
                }

                player.Draw();
                agent.Draw();
                agent1.Draw();
                agent2.Draw();

                spriteBatch.Begin();
                spriteBatch.DrawString(font, "HITS: " + hits, new Vector2(50, 50), Color.Red);
                spriteBatch.DrawString(font, "Time: " + Time.TotalGameTime, new Vector2(70, 70), Color.Red);
                spriteBatch.End();

                base.Draw(gameTime);
            }
        }
    }
}
