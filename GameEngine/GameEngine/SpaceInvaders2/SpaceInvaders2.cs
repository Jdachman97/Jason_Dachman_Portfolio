using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using CPI311.GameEngine;
using CPI311.GameEngine.Rendering;

namespace SpaceInvaders2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceInvaders2 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        Transform cameraTransform;
        Light light;

        //Audio components
        SoundEffect gunSound;
        SoundEffect enemyGunSound;
        SoundEffectInstance soundInstance;
        SoundEffectInstance soundInstance2;
        SoundEffect explosion2;
        SoundEffect explosion3;
        SoundEffect explosion4;
        SoundEffect BackGroundMusic;
        SoundEffect success;
        SoundEffect failure;

        //Visual components
        Player player;
        Shield[] shieldList = new Shield[Constants.numShields];
        Enemy[] enemylist = new Enemy[Constants.NumEnemies];
        Bullet[] bulletList = new Bullet[Constants.NumBullets];
        EnemyBullet[] enemyBulletList = new EnemyBullet[Constants.NumBullets];
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

        //mechanics
        float difficultyModifier = 1.0f;
        int shootSleep = 0;
        int enemyShootSleep = 0;
        bool gameWin = false;
        bool gameLose = false;
        bool losssoundHasPlayed = false;
        bool winSOundHasPlayed = false;
        int deadEnemies = 0;




        public SpaceInvaders2()
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
            camera.FarPlane = 30000.0f;
            camera.Transform = cameraTransform;
            cameraTransform.LocalPosition = Vector3.Up * 10000;
            cameraTransform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            random = new Random();
            gunSound = Content.Load<SoundEffect>("laser-shot-silenced");
            enemyGunSound = Content.Load<SoundEffect>("laser4");
            explosion2 = Content.Load<SoundEffect>("explosion2");
            explosion3 = Content.Load<SoundEffect>("explosion3");
            explosion3 = Content.Load<SoundEffect>("explosion3");
            explosion4 = Content.Load<SoundEffect>("explosion00");
            BackGroundMusic = Content.Load<SoundEffect>("laserattack");
            success = Content.Load<SoundEffect>("success3");
            failure = Content.Load<SoundEffect>("failure2");
            soundInstance2 = BackGroundMusic.CreateInstance();
            soundInstance2.IsLooped = true;
            soundInstance2.Volume = 0.3f;
            soundInstance2.Play();
            kootenay = Content.Load<SpriteFont>("Kootenay");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(Content, camera, GraphicsDevice, light);
            player.Transform.LocalPosition = new Vector3(0, 0, Constants.PlayfieldSizeY - 3200);
        
         
            
            for (int i = 0; i < Constants.NumBullets; i++)
            {
                bulletList[i] = new Bullet(Content, camera, GraphicsDevice, light);
                enemyBulletList[i] = new EnemyBullet(Content, camera, GraphicsDevice, light);
            }

            for (int i = 0; i < Constants.numShields; i++)
            {
                shieldList[i] = new Shield(Content, camera, GraphicsDevice, light);
              
            }
            ResetEnemies(); // look at the below private method
            ResetShields();

            // *** Particle
            particleManager = new ParticleManager(GraphicsDevice, 100);
            particleEffect = Content.Load<Effect>("ParticleShader-complete");
            particleTex = Content.Load<Texture2D>("fire explosion");

            stars = Content.Load<Texture2D>("space4");


            



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
            player.Update();

          

            shootSleep++;
            enemyShootSleep++;

            for (int i = 0; i < Constants.NumBullets; i++)
            {
                bulletList[i].Update();
            }
            for (int i = 0; i < Constants.numShields; i++)
            {
                shieldList[i].Update();
            }
            Vector3 normal;
            for (int i = 0; i < Constants.NumEnemies; i++)
            {
                //randomly spawn enemy bullets
                if (random.Next(0, 28 * enemylist.Length) < 2 && enemyShootSleep > 85 && enemylist[i].isActive)
                {
                    for (int j = 0; j < Constants.NumBullets; j++)
                    {
                        enemyShootSleep = 0;
                        if (!enemyBulletList[j].isActive)
                        {
                            enemyBulletList[j].Rigidbody.Velocity = Vector3.Backward * Constants.enemyBulletSpeed;
                            enemyBulletList[j].Transform.LocalPosition = enemylist[i].Transform.Position + (200 * enemyBulletList[j].Transform.Forward);
                            enemyBulletList[j].isActive = true;
                            
                            // sound
                            soundInstance = enemyGunSound.CreateInstance();
                            soundInstance.Play();
                            
                            break; //exit the loop     
                        }
                    }
                }
                enemylist[i].Update();
                enemyBulletList[i].Update();
            }


            //player shoot
                if (InputManager.IsKeyPressed (Keys.Space) && shootSleep > 45)
                {
                shootSleep = 0;
                for (int i = 0; i < Constants.NumBullets; i++)
                {
                    
                    if (!bulletList[i].isActive)
                    {
                        bulletList[i].Rigidbody.Velocity = (player.Collider.Transform.Forward ) * Constants.BulletSpeedAdjustment;
                        bulletList[i].Transform.LocalPosition = player.Collider.Transform.Position + (200 * bulletList[i].Transform.Forward);
                        bulletList[i].isActive = true;
                        
                        // sound
                        soundInstance = gunSound.CreateInstance();
                        soundInstance.Play();
                       
                        break; //exit the loop     
                    }
                }
            }

            // enemy bullet collision
           
            for (int i = 0; i < enemylist.Length; i++)
                if (enemylist[i].isActive)
                    for (int j = 0; j < bulletList.Length; j++)
                        if (bulletList[j].isActive)
                            if (enemylist[i].Collider.Collides(bulletList[j].Collider, out normal))
                            {
                                // Particles
                                Particle particle = particleManager.getNext();
                                particle.Position = enemylist[i].Transform.Position;
                                particle.Velocity = new Vector3(
                                  random.Next(-5, 5), 2, random.Next(-50, 50));
                                particle.Acceleration = new Vector3(0, 3, 0);
                                particle.MaxAge = random.Next(1, 6);
                                particle.Color = new Vector3(0.1F, 0.1F, 0.1F);
                                particle.Init();
                                enemylist[i].isActive = false;
                                bulletList[j].isActive = false;
                                difficultyModifier *= 1.05f;
                                deadEnemies++;
                                soundInstance = explosion4.CreateInstance();
                                soundInstance.Play();
                                score += enemylist[i].scoreValue;
                                break; //no need to check other bullets
                            }

            //enemy ship collision
            for (int i = 0; i < enemylist.Length; i++)
                if (enemylist[i].isActive)

                    if (player.isActive)
                        if (enemylist[i].Collider.Collides(player.Collider, out normal))
                        {
                            // Particles
                            Particle particle = particleManager.getNext();
                            particle.Position = enemylist[i].Transform.Position;
                            particle.Velocity = new Vector3(
                              random.Next(-5, 5), 2, random.Next(-50, 50));
                            particle.Acceleration = new Vector3(0, 3, 0);
                            particle.MaxAge = random.Next(1, 6);
                            particle.Init();
                            enemylist[i].isActive = false;
                            player.isActive = false;
                            soundInstance = explosion4.CreateInstance();
                            soundInstance.Play();
                            Console.WriteLine("Ope");
                            score -= Constants.DeathPenalty;

                        }

            //enemy barrier collision
            for (int i = 0; i < enemylist.Length; i++)
            {


                if (enemylist[i].Transform.LocalPosition.X > Constants.PlayfieldSizeX)
                {
                    for (int j = 0; j < enemylist.Length; j++)
                    {
                        enemylist[j].movementDirection = -1;
                        enemylist[j].Rigidbody.Velocity = new Vector3(1600 * enemylist[j].movementDirection * difficultyModifier, 0, 0);

                    }
                   
                    //break;
                }

                if (enemylist[i].Transform.LocalPosition.X < -Constants.PlayfieldSizeX)
                {
                    for (int j = 0; j < enemylist.Length; j++)
                    {
                        enemylist[j].movementDirection = 1;
                        enemylist[j].Transform.LocalPosition += Vector3.Backward * 180;
                        enemylist[j].Rigidbody.Velocity = new Vector3(1600 * enemylist[j].movementDirection * difficultyModifier, 0, 0);

                    }
                    
                   // break;
                }

                else
                {
                    enemylist[i].Rigidbody.Velocity = new Vector3(1600 * enemylist[i].movementDirection * difficultyModifier, 0, 0);
                }
                

            }

            //ship bullet collision
            for (int i = 0; i < enemyBulletList.Length; i++)
                if (enemyBulletList[i].isActive)

                    if (player.isActive)
                        if (enemyBulletList[i].Collider.Collides(player.Collider, out normal))
                        {
                            player.lives -= 1;
                            // Particles
                            Particle particle = particleManager.getNext();
                            particle.Position = enemyBulletList[i].Transform.Position;
                            particle.Velocity = new Vector3(
                              random.Next(-5, 5), 2, random.Next(-50, 50));
                            particle.Acceleration = new Vector3(0, 3, 0);
                            particle.MaxAge = random.Next(1, 6);
                            particle.Init();
                            enemyBulletList[i].isActive = false;
                            if (player.lives <= 0)
                            {
                                player.isActive = false;
                            }
                            soundInstance = explosion4.CreateInstance();
                            soundInstance.Play();
                            Console.WriteLine("Ope");
                            score -= Constants.DeathPenalty;

                        }

            //enemy bullet shield collision
            for (int i = 0; i < enemyBulletList.Length; i++)
                if (enemyBulletList[i].isActive)
                    for (int j = 0; j < shieldList.Length; j++) {

                        if (shieldList[j].isActive)
                        {
                            if (enemyBulletList[i].Collider.Collides(shieldList[j].Collider, out normal) || enemyBulletList[i].Collider.Collides(shieldList[j].Collider, out normal) || enemyBulletList[i].Collider.Collides(shieldList[j].Collider, out normal))
                            {

                                // Particles
                                Particle particle = particleManager.getNext();
                                particle.Position = enemyBulletList[i].Transform.Position;
                                particle.Velocity = new Vector3(
                                  random.Next(-5, 5), 2, random.Next(-50, 50));
                                particle.Acceleration = new Vector3(0, 3, 0);
                                particle.MaxAge = random.Next(1, 6);
                                particle.Init();
                                enemyBulletList[i].isActive = false;
                                shieldList[j].isActive = false;
                                soundInstance = explosion4.CreateInstance();
                                soundInstance.Play();
                                Console.WriteLine("Ope");

                            }
                        }
                    }

            //player bullet shield collision
            for (int i = 0; i < bulletList.Length; i++)
                if (bulletList[i].isActive)
                    for(int j = 0; j < shieldList.Length; j++ ) {
                        if (shieldList[j].isActive)
                        {
                            if (bulletList[i].Collider.Collides(shieldList[j].Collider, out normal) || bulletList[i].Collider.Collides(shieldList[j].Collider, out normal) || bulletList[i].Collider.Collides(shieldList[j].Collider, out normal))
                            {
                                Console.WriteLine("Ope");
                                // Particles
                                Particle particle = particleManager.getNext();
                                particle.Position = bulletList[i].Transform.Position;
                                particle.Velocity = new Vector3(
                                  random.Next(-5, 5), 2, random.Next(-50, 50));
                                particle.Acceleration = new Vector3(0, 3, 0);
                                particle.MaxAge = random.Next(1, 6);
                                particle.Init();
                                bulletList[i].isActive = false;
                                shieldList[j].isActive = false;
                                soundInstance = explosion4.CreateInstance();
                                soundInstance.Play();


                            }
                        }
                    }

            //enemy shield collision
            for (int i = 0; i < enemylist.Length; i++)
                if (enemylist[i].isActive)
                    for (int j = 0; j < shieldList.Length; j++)
                    {
                        if (shieldList[j].isActive)
                        {
                            if (enemylist[i].Collider.Collides(shieldList[j].Collider, out normal) )
                            {
                               
                                // Particles
                                Particle particle = particleManager.getNext();
                                particle.Position = bulletList[i].Transform.Position;
                                particle.Velocity = new Vector3(
                                  random.Next(-5, 5), 2, random.Next(-50, 50));
                                particle.Acceleration = new Vector3(0, 3, 0);
                                particle.MaxAge = random.Next(1, 6);
                                particle.Init();
                                shieldList[j].isActive = false;
                                soundInstance = explosion4.CreateInstance();
                                soundInstance.Play();


                            }
                        }
                    }


            // particles update
            particleManager.Update();

            //lose
            if (!player.isActive)
            {
                gameLose = true;
                if (!losssoundHasPlayed)
                {

                    losssoundHasPlayed = true;
                    soundInstance = failure.CreateInstance();
                    soundInstance.Play();
                }
                for (int i = 0; i < Constants.NumBullets; i++)
                {
                    bulletList[i].isActive = false;
                    
                }

            }

            //win
            if (deadEnemies >= 50)
            {
                gameWin = true;
                if (!winSOundHasPlayed)
                {
                    winSOundHasPlayed = true;
                    soundInstance = success.CreateInstance();
                    soundInstance.Play();
                }

               
                
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            spriteBatch.Begin();
            spriteBatch.Draw(stars, new Rectangle(0, 0, 2100, 1200), Color.White);
            spriteBatch.End();
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            // ship, bullets, and asteroids
            player.Draw();
 
            for (int i = 0; i < Constants.numShields; i++) shieldList[i].Draw();
            for (int i = 0; i < Constants.NumBullets; i++) bulletList[i].Draw();
            for (int i = 0; i < Constants.NumEnemies; i++) enemylist[i].Draw();
            for (int i = 0; i < Constants.NumBullets; i++) enemyBulletList[i].Draw();

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
                                   new Vector2(1600, 30), Color.LightGreen);
            spriteBatch.DrawString(kootenay, "Lives: " + player.lives,
                                    new Vector2(1750, 30), Color.LightGreen);
            spriteBatch.DrawString(kootenay, "Use A to move left and D to move right",
                                  new Vector2(50, 30), Color.LightGreen);
            spriteBatch.DrawString(kootenay, "Use spacebar to shoot",
                                 new Vector2(50, 60), Color.LightGreen);
            if (gameWin)
            {
                spriteBatch.DrawString(kootenay, "You Win! ",
                                    new Vector2(860, 440), Color.White);
            }
            if (gameLose)
            {
                spriteBatch.DrawString(kootenay, "You Lose! ",
                                        new Vector2(860, 440), Color.White);
            }

            if(gameWin || gameLose)
            {
                spriteBatch.DrawString(kootenay, "All code written by:  Jason Dachman ",
                                       new Vector2(860, 480), Color.White);
                spriteBatch.DrawString(kootenay, "All art assets retrieved from:  TurboSquid.com ",
                                       new Vector2(860, 520), Color.White);
                spriteBatch.DrawString(kootenay, "All art sounds retrieved from:  Freesound.org ",
                                       new Vector2(860, 560), Color.White);


            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ResetEnemies()
        {
            float xStart;
            float yStart;
            
            for (int i = 0; i < Constants.NumEnemies; i++)
            {  
                xStart = -(float)Constants.PlayfieldSizeX + 100;
                yStart = -(float)Constants.PlayfieldSizeY + 3750;
                enemylist[i] = new Enemy(Content, camera, GraphicsDevice, light);
                enemylist[i].movementDirection = 1;
               
                
                enemylist[i].Transform.Position = new Vector3(xStart + (i * Constants.enemySpacerX), 0.0f, yStart);
                if (i >= 10)
                {
                    enemylist[i].Transform.Position = new Vector3(xStart + (i % 10 * Constants.enemySpacerX), 0.0f, yStart + 1500);
                    enemylist[i].scoreValue = 2000;
                }

                if (i >= 20)
                {
                    enemylist[i].Transform.Position = new Vector3(xStart + (i % 10 * Constants.enemySpacerX), 0.0f, yStart + 3000);
                    enemylist[i].scoreValue = 1500;
                }

                if (i >= 30)
                {
                    enemylist[i].Transform.Position = new Vector3(xStart + (i % 10 * Constants.enemySpacerX), 0.0f, yStart + 4500);
                    enemylist[i].scoreValue = 1000;
                }

                if (i >= 40)
                {
                    enemylist[i].Transform.Position = new Vector3(xStart + (i % 10 * Constants.enemySpacerX), 0.0f, yStart + 6000);
                    enemylist[i].scoreValue = 500;
                }

                else
                {
                    enemylist[i].scoreValue = 2500;
                }

                enemylist[i].isActive = true;
            }

          
        }

        private void ResetShields()
        {
            float xStart;
            float yStart;

            for (int i = 0; i < Constants.numShields; i++)
            {
                xStart = -(float)Constants.PlayfieldSizeX + 1200;
                yStart = (float)Constants.PlayfieldSizeY - 5900;
                shieldList[i] = new Shield(Content, camera, GraphicsDevice, light); 
                shieldList[i].Transform.LocalScale = new Vector3(11, 1, 6);

                shieldList[i].isActive = true;

                shieldList[i].Transform.Position = new Vector3(xStart, 0.0f, yStart - (i % 3 * Constants.shieldSpacerY));
                if (i >= 3)
                {
                    shieldList[i].Transform.Position = new Vector3(xStart + 11500, 0.0f, yStart - (i % 3 * Constants.shieldSpacerY));
                    
                }

                if (i >= 6)
                {
                    shieldList[i].Transform.Position = new Vector3(xStart + 23000, 0.0f, yStart - (i % 3 * Constants.shieldSpacerY));
                    
                }

            }


        }


    }
}
