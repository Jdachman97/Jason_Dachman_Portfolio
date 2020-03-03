using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using CPI311.GameEngine;
using CPI311.GameEngine.Rendering;

namespace Assignment03
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Assignment03 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool haveThreadRunning = true;
        bool checkForSpheres = false;
        bool speedColor = false;
        int lastSecondCollisions = 1;
        int numberCollisions = 1;
        float speed = 0;
        float speedValue = 0;

        Model Model;
        SpriteFont Font;
        Camera Camera;
        Transform cameraTransform;
        Random random;
        Light Light;
        Transform LightTransform;
        String filename;
        List<Renderer> renderers;
        List<GameObject> GameObjects;
        BoxCollider boxCollider;
        float adjSpeed = 1.0f;
        bool drawText = false;
        float frameRate;
        List<int> totalCollisions;
        int collisionSum = 0;
        public Assignment03()
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

            Time.Initialize();
            InputManager.Initialize();
            this.IsMouseVisible = true;
            // TODO: Add your initialization logic here
            ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));
            //transforms = new List<Transform>();
            //rigidbodies = new List<Rigidbody>();
            //Colliders = new List<Collider>();
            GameObjects = new List<GameObject>();
            boxCollider = new BoxCollider();
            renderers = new List<Renderer>();
            Light = new Light();
            totalCollisions = new List<int>();
            LightTransform = new Transform();
            Light.Transform = LightTransform;
            boxCollider.Size = 10;
            random = new Random();

            

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

            Model = Content.Load<Model>("Sphere");
            Font = Content.Load<SpriteFont>("File");
            Camera = new Camera();
            cameraTransform = new Transform();
            Camera.Transform = cameraTransform;
            cameraTransform.LocalPosition = Vector3.Backward * 25;

            AddGameObject();
            AddGameObject();
            AddGameObject();
            AddGameObject();
            AddGameObject();


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

            totalCollisions.Add(lastSecondCollisions);
            if (totalCollisions.Count > 5)
            {
                totalCollisions.RemoveAt(0);
            }
            collisionSum = (totalCollisions.Sum() / totalCollisions.Count());
           

            Vector3 normal; // it is updated if a collision happens
              for (int i = 0; i < GameObjects.Count; i++)
              {
                  if (boxCollider.Collides(GameObjects[i].Collider, out normal))
                  {
                      numberCollisions++;
                      if (Vector3.Dot(normal, GameObjects[i].Rigidbody.Velocity) < 0)
                          GameObjects[i].Rigidbody.Impulse +=
                             Vector3.Dot(normal, GameObjects[i].Rigidbody.Velocity) * -2 * normal;
                  }
               /* if (checkForSpheres == true)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(checkSphereCollision));
                }
                else
                {*/
                    for (int j = i + 1; j < GameObjects.Count; j++)
                    {
                        if (GameObjects[i].Collider.Collides(GameObjects[j].Collider, out normal))
                        {
                            numberCollisions++;

                            Vector3 velocityNormal = Vector3.Dot(normal,
                                GameObjects[i].Rigidbody.Velocity - GameObjects[j].Rigidbody.Velocity) * -2
                                   * normal * GameObjects[i].Rigidbody.Mass * GameObjects[j].Rigidbody.Mass;
                            GameObjects[i].Rigidbody.Impulse += velocityNormal / 2;
                            GameObjects[j].Rigidbody.Impulse += -velocityNormal / 2;
                        }
                    }
               // }
              }



            foreach (GameObject gameObject in GameObjects)
            {
                speed = gameObject.Rigidbody.Velocity.Length();
                speedValue = MathHelper.Clamp(speed / 20f, 0, 1);
                gameObject.Rigidbody.Velocity *= adjSpeed;
                gameObject.Update();
               // Console.WriteLine(gameObject.Transform.Position);
            }

            if (InputManager.IsKeyPressed(Keys.Up))
            {
                AddGameObject();

            }

            if (InputManager.IsKeyPressed(Keys.Down))
            {
                GameObjects.RemoveAt(0);

            }

            if (InputManager.IsKeyPressed(Keys.Right))
            {
                adjSpeed *= 1.03f; 
            }

            if (InputManager.IsKeyPressed(Keys.Left))
            {
                adjSpeed *= 0.97f;
            }

            if (InputManager.IsKeyDown(Keys.LeftShift))
            {
                drawText = true;
            }
            else drawText = false;

            if (InputManager.IsKeyPressed(Keys.Space))
            {
                speedColor = true;
            }
            else speedColor = false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here
             //foreach (Transform transform in transforms)
               //  Model.Draw(transform.World, Camera.View, Camera.Projection);
            // for (int i = 0; i < GameObjects.Count; i++) renderers[i].Draw();
             /*
            for (int i = 0; i < transforms.Count; i++)
            {
                Transform transform = transforms[i];
                float speed = rigidbodies[i].Velocity.Length();
                float speedValue = MathHelper.Clamp(speed / 20f, 0, 1);
                (Model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                                           new Vector3(speedValue, speedValue, 1);
                Model.Draw(transform.World, Camera.View, Camera.Projection);
            }*/

            foreach (GameObject gameObject in GameObjects)
            {

               // if (speedColor == true)
               // {
                    gameObject.Renderer.Light.Ambient = new Color(speedValue, speedValue, 1.0f, 1.0f);
                    gameObject.Renderer.Light.Diffuse = new Color(speedValue, speedValue, 1.0f, 1.0f);
                    gameObject.Renderer.Light.Specular = new Color(speedValue, speedValue, 1.0f, 1.0f);
                    gameObject.Draw();
                //}
               /* else
                {
                    gameObject.Renderer.Light.Ambient = new Color(0.1f, 0.1f, 1.0f, 1.0f);
                    gameObject.Renderer.Light.Diffuse = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                    gameObject.Renderer.Light.Specular = new Color(0.3f, 0.3f, 1.0f, 1.0f);
                    gameObject.Draw();
                }*/
                
            }
           /* for (int i = 0; i < GameObjects.Count; i++)
            {
                float speed = GameObjects[i].Rigidbody.Velocity.Length();
                float speedValue = MathHelper.Clamp(speed / 20f, 0, 1);

                GameObjects[i].Renderer.Light.Ambient = new Color(speedValue, speedValue, 1.0f, 1.0f);
                GameObjects[i].Renderer.Light.Diffuse = new Color(speedValue, speedValue, 1.0f, 1.0f);
                GameObjects[i].Renderer.Light.Specular = new Color(speedValue, speedValue, 1.0f, 1.0f);
                GameObjects[i].Renderer.Draw();
            }*/

            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.Begin();
            if (drawText == true)
            {
                spriteBatch.DrawString(Font,
                "Total Balls: " + GameObjects.Count.ToString(),
                new Vector2(10, 20),
                Color.White
                );

                spriteBatch.DrawString(Font,
               "totalFrameRate: " + Math.Round(frameRate),
               new Vector2(10, 50),
               Color.White
               );

                spriteBatch.DrawString(Font,
             "Average number of Collisions: " + collisionSum,
             new Vector2(10, 80),
             Color.White
             );

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void CollisionReset(object obj)
        {
            while (haveThreadRunning)
            {
                lastSecondCollisions = numberCollisions;
                System.Threading.Thread.Sleep(5000);

                numberCollisions = 0;
                
            }
        }

       /* private void checkSphereCollision(object obj)
        {
            
                for (int j = i + 1; j < GameObjects.Count; j++)
                {
                    if (GameObjects[i].Collider.Collides(GameObjects[j].Collider, out normal))
                    {
                        numberCollisions++;

                        Vector3 velocityNormal = Vector3.Dot(normal,
                            GameObjects[i].Rigidbody.Velocity - GameObjects[j].Rigidbody.Velocity) * -2
                               * normal * GameObjects[i].Rigidbody.Mass * GameObjects[j].Rigidbody.Mass;
                        GameObjects[i].Rigidbody.Impulse += velocityNormal / 2;
                        GameObjects[j].Rigidbody.Impulse += -velocityNormal / 2;
                    }
                }
            
        }
        */
        private void AddGameObject() 
        {
            

            Transform transform = new Transform();
            transform.LocalPosition += new Vector3 ((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()) * (float)random.NextDouble() * 9; //avoid overlapping each sphere 
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = transform;
            rigidbody.Mass = 1.0f + (float)random.NextDouble();


            Light light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalScale = Vector3.One * 2f;
            float speed = rigidbody.Velocity.Length();
            float speedValue = MathHelper.Clamp(speed / 20f, 0, 1);
            
         //   light.Ambient = new Color(0.5f, 1.0f, 1.0f, 1.0f);
          //  light.Diffuse = new Color(0.5f, 1.0f, 1.0f, 1.0f) ;
           // light.Specular = new Color(0.5f, 1.0f, 1.0f, 1.0f);
        

            Vector3 direction = new Vector3(
              (float)random.NextDouble(), (float)random.NextDouble(),
              (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity =
               direction * ((float)random.NextDouble() * 5 + 5);
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1.0f * transform.LocalScale.Y;
            sphereCollider.Transform = transform;

            

            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Model, transform, Camera, Content,
                            GraphicsDevice, Light, 0, 20f, texture, "SimpleShading2");



            renderers.Add(renderer);
            GameObject gameObject = new GameObject();
            gameObject.Add<Rigidbody>(rigidbody);
            gameObject.Add<Collider>(sphereCollider);
            gameObject.Add<Renderer>(renderer);
           
            // transforms.Add(transform);
            //Colliders.Add(sphereCollider);
            //rigidbodies.Add(rigidbody);

            GameObjects.Add(gameObject);
            
        }

    }
}
