using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Collections.Generic;
using CPI311.GameEngine;
using CPI311.GameEngine.Rendering;

namespace Lab07
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab07 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool haveThreadRunning = true;
        int lastSecondCollisions = 0;
        int numberCollisions = 0;
        Model Model;
        SpriteFont Font;
        Camera Camera;
        Transform cameraTransform;
        Random random;
        Light Light;
        Transform LightTransform;
        String filename;
        List<Renderer> renderers;
        List<Transform> transforms;
        List<Rigidbody> rigidbodies;
        List<Collider> Colliders;
        BoxCollider boxCollider;
        SphereCollider sphere1, sphere2;

        public Lab07()
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
            // TODO: Add your initialization logic here
           // ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadMethod));
            ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));
            transforms = new List<Transform>();
            rigidbodies = new List<Rigidbody>();
            Colliders = new List<Collider>();
            boxCollider = new BoxCollider();
            renderers = new List<Renderer>();
            Light = new Light();
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
           
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Model = Content.Load<Model>("Sphere");
            Font = Content.Load<SpriteFont>("File");
            Camera = new Camera();
            cameraTransform = new Transform();
            Camera.Transform = cameraTransform;
            cameraTransform.LocalPosition = Vector3.Backward * 25;
            AddSphere();
             sphere1 = new SphereCollider();
            sphere2 = new SphereCollider();

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

            foreach (Rigidbody rigidbody in rigidbodies) rigidbody.Update();

            Vector3 normal; // it is updated if a collision happens
            for (int i = 0; i < transforms.Count; i++)
            {
                if (boxCollider.Collides(Colliders[i], out normal))
                {
                    numberCollisions++;
                    if (Vector3.Dot(normal, rigidbodies[i].Velocity) < 0)
                        rigidbodies[i].Impulse +=
                           Vector3.Dot(normal, rigidbodies[i].Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < transforms.Count; j++)
                {
                    if (Colliders[i].Collides(Colliders[j], out normal))
                    {
                        numberCollisions++;

                        Vector3 velocityNormal = Vector3.Dot(normal,
                            rigidbodies[i].Velocity - rigidbodies[j].Velocity) * -2
                               * normal * rigidbodies[i].Mass * rigidbodies[j].Mass;
                        rigidbodies[i].Impulse += velocityNormal / 2;
                        rigidbodies[j].Impulse += -velocityNormal / 2;
                    }
                }
            }
            if (InputManager.IsKeyPressed(Keys.Space))
            {
                AddSphere();

            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here

            for (int i = 0; i < renderers.Count; i++)
            {
                float speed = rigidbodies[i].Velocity.Length();
                float speedValue = MathHelper.Clamp(speed / 20f, 0, 1);
                
                renderers[i].Light.Ambient = new Color(speedValue, speedValue, 1.0f, 1.0f);
                renderers[i].Light.Diffuse = new Color(speedValue, speedValue, 1.0f, 1.0f);
                renderers[i].Light.Specular = new Color(speedValue, speedValue, 1.0f, 1.0f);
                renderers[i].Draw();
            }


            spriteBatch.Begin();
            spriteBatch.DrawString(Font,
            numberCollisions.ToString(),
            new Vector2(10, 25),
            Color.White
            );
            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void CollisionReset(object obj)
        {
            while (haveThreadRunning)
            {
                lastSecondCollisions = numberCollisions;
                numberCollisions = 0;
                System.Threading.Thread.Sleep(1000);
            }
        }
        private void AddSphere()
        {

            Transform transform = new Transform();
            transform.LocalPosition += Vector3.Right * 5; //avoid overlapping each sphere 
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = transform;
           
            rigidbody.Mass = 1;

            Vector3 direction = new Vector3(
              (float)random.NextDouble(), (float)random.NextDouble(),
              (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity =
               direction * ((float)random.NextDouble() * 5 + 5);
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1.0f * transform.LocalScale.Y;
            sphereCollider.Transform = transform;

            Light light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalScale = Vector3.One * 2f;
            float speed = rigidbody.Velocity.Length();
            float speedValue = MathHelper.Clamp(speed / 20f, 0, 1);
          //  Console.WriteLine(speedValue);
            light.Ambient = new Color(speedValue, speedValue, 1.0f, 1.0f);
            light.Diffuse = new Color(speedValue, speedValue, 1.0f, 1.0f);
            light.Specular = new Color(speedValue, speedValue, 1.0f, 1.0f);

            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Model, transform, Camera, Content,
                            GraphicsDevice, light, 0, 20f, texture, "SimpleShading2");
            //renderer.transform.LocalScale = Vector3.One * 2f;
            renderers.Add(renderer);

            


            transforms.Add(transform);
            Colliders.Add(sphereCollider);
            rigidbodies.Add(rigidbody);
            Console.WriteLine("Sphere");
        }



    }
}
