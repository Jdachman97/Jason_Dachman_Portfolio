using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using CPI311.GameEngine;
using CPI311.GameEngine.Rendering;

namespace Assignment4
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Assignment4 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        Transform cameraTransform;
        Light light;

        //Audio components
        SoundEffect gunSound;
        SoundEffectInstance soundInstance;
        SoundEffect explosion2;
        SoundEffect explosion3;

        //Visual components
        Ship ship;
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Bullet[] bulletList = new Bullet[GameConstants.NumBullets];
        SpriteFont kootenay;

        //Score & background
        int score;
        Texture2D stars;
        SpriteFont lucidaConsole;
        Vector2 scorePosition = new Vector2(100, 50);

        // Particles
        ParticleManager particleManager;
        Texture2D particleTex;
        Effect particleEffect;

        Random random;

        //pathfinding
        bool isFollowing;


        public Assignment4()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
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
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(graphics);
            ScreenManager.Setup(true, 1920, 1080);
           /* search = new AStarSearch((int)GameConstants.PlayfieldSizeX, (int)GameConstants.PlayfieldSizeY);

            search.Start = search.Nodes[0, 0];
            search.Start.Passable = true;
            search.End = search.Nodes[(int)ship.Transform.Position.X, (int)ship.Transform.Position.Y];
            search.End.Passable = true;


            search.Search(); // A search is made here.

            path = new List<Vector3>();
            //exctract list of path from End-node by using parent
            AStarNode current = search.End;
            while (current != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
            }*/
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            camera = new Camera();
            cameraTransform = new Transform();
            camera.NearPlane = 1.0f;
            camera.FarPlane = 10000.0f;
            camera.Transform = cameraTransform;
            cameraTransform.LocalPosition = Vector3.Up * 5000;
            cameraTransform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            random = new Random();
            gunSound = Content.Load<SoundEffect>("tx0_fire1");
            explosion2 = Content.Load<SoundEffect>("explosion2");
            explosion3 = Content.Load<SoundEffect>("explosion3");
            kootenay = Content.Load<SpriteFont>("Kootenay");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ship = new Ship(Content, camera, GraphicsDevice, light);
            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                bulletList[i] = new Bullet(Content, camera, GraphicsDevice, light);
            }
            ResetAsteroids(); // look at the below private method

            // *** Particle
            particleManager = new ParticleManager(GraphicsDevice, 100);
            particleEffect = Content.Load<Effect>("ParticleShader-complete");
            particleTex = Content.Load<Texture2D>("fire");

            stars = Content.Load<Texture2D>("B1_stars");


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
            ship.Update();

           
            

            for (int i = 0; i < GameConstants.NumBullets; i++)
                bulletList[i].Update();
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (asteroidList[i].isFollowing)
                {
                    Vector3 direction = (ship.Transform.Position - asteroidList[i].Transform.Position) / (float)(asteroidList[i].followingModifier) ;
                    asteroidList[i].Rigidbody.Velocity = direction;
                }
                asteroidList[i].Update();
            }

            if (InputManager.IsMousePressed(0))
            {
                for (int i = 0; i < GameConstants.NumBullets; i++)
                {
                    if (!bulletList[i].isActive)
                    {
                        bulletList[i].Rigidbody.Velocity = (ship.Transform.Forward) * GameConstants.BulletSpeedAdjustment;
                        bulletList[i].Transform.LocalPosition = ship.Transform.Position + (200 * bulletList[i].Transform.Forward);
                        bulletList[i].isActive = true;
                        score -= GameConstants.ShotPenalty;
                        // sound
                        soundInstance = gunSound.CreateInstance();
                        soundInstance.Play();
                        break; //exit the loop     
                    }
                }
            }

            // asteroid bullet collision
            Vector3 normal;
            for (int i = 0; i < asteroidList.Length; i++)
                if (asteroidList[i].isActive)
                    for (int j = 0; j < bulletList.Length; j++)
                        if (bulletList[j].isActive)
                            if (asteroidList[i].Collider.Collides(bulletList[j].Collider, out normal))
                            {
                                // Particles
                                Particle particle = particleManager.getNext();
                                particle.Position = asteroidList[i].Transform.Position;
                                particle.Velocity = new Vector3(
                                  random.Next(-5, 5), 2, random.Next(-50, 50));
                                particle.Acceleration = new Vector3(0, 3, 0);
                                particle.MaxAge = random.Next(1, 6);
                                particle.Init();
                                asteroidList[i].isActive = false;
                                bulletList[j].isActive = false;
                                soundInstance = explosion2.CreateInstance();
                                soundInstance.Play();
                                score += GameConstants.KillBonus;
                                break; //no need to check other bullets
                            }
            
            //asteroid ship collision
            for (int i = 0; i < asteroidList.Length; i++)
                if (asteroidList[i].isActive)
                    
                        if (ship.isActive)
                            if (asteroidList[i].Collider.Collides(ship.Collider, out normal))
                            {
                                // Particles
                                Particle particle = particleManager.getNext();
                                particle.Position = asteroidList[i].Transform.Position;
                                particle.Velocity = new Vector3(
                                  random.Next(-5, 5), 2, random.Next(-50, 50));
                                particle.Acceleration = new Vector3(0, 3, 0);
                                particle.MaxAge = random.Next(1, 6);
                                particle.Init();
                                asteroidList[i].isActive = false;
                                ship.isActive = false;
                                soundInstance = explosion3.CreateInstance();
                                soundInstance.Play();
                                Console.WriteLine("Ope");
                                score -= GameConstants.DeathPenalty;
                               
                            }
            // particles update
            particleManager.Update();


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

            // TODO: Add your drawing code here
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            spriteBatch.Begin();
            spriteBatch.Draw(stars, new Rectangle(0, 0, 2000, 1100), Color.White);
            spriteBatch.End();
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            // ship, bullets, and asteroids
            ship.Draw();
            for (int i = 0; i < GameConstants.NumBullets; i++) bulletList[i].Draw();
            for (int i = 0; i < GameConstants.NumAsteroids; i++) asteroidList[i].Draw();

            //particle draw
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            particleEffect.CurrentTechnique = particleEffect.Techniques["particle"];
            particleEffect.CurrentTechnique.Passes[0].Apply();
            particleEffect.Parameters["ViewProj"].SetValue(camera.View * camera.Projection);
            particleEffect.Parameters["World"].SetValue(Matrix.Identity);

            particleEffect.Parameters["CamIRot"].SetValue(
            Matrix.Invert(Matrix.CreateFromQuaternion(camera.Transform.Rotation)));
            particleEffect.Parameters["Texture"].SetValue(particleTex);
            particleManager.Draw(GraphicsDevice);
           // ...

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;


            spriteBatch.Begin();
            spriteBatch.DrawString(kootenay, "Score: " + score,
                                   scorePosition, Color.LightGreen);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ResetAsteroids()
        {
            float xStart;
            float yStart;
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (random.Next(2) == 0)
                {
                    
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                    xStart = (float)GameConstants.PlayfieldSizeX;

                yStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                asteroidList[i] = new Asteroid(Content, camera, GraphicsDevice, light);
                asteroidList[i].Transform.Position = new Vector3(xStart, 0.0f, yStart);
                double angle = random.NextDouble() * 2 * Math.PI;
                asteroidList[i].Rigidbody.Velocity = new Vector3(
                   -(float)Math.Sin(angle), 0, (float)Math.Cos(angle)) *
            (GameConstants.AsteroidMinSpeed + (float)random.NextDouble() *
            GameConstants.AsteroidMaxSpeed);

                if (random.Next(2) == 0)
                {
                    asteroidList[i].isFollowing = true;
                    asteroidList[i].followingModifier = (random.NextDouble() * 10) + 4;
                }

                else
                {
                    asteroidList[i].isFollowing = false;
                }

                asteroidList[i].isActive = true;
            }
        }

    }
}
