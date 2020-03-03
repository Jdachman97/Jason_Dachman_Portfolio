using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System;
using System.Collections.Generic;

namespace Lab06
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab06 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BoxCollider boxCollider;
        SphereCollider sphere1, sphere2;
        Random random;
        List<Transform> transforms;
        List<Rigidbody> rigidbodies;
        List<Collider> Colliders;
        Transform cameraTransform;
        Camera Camera;
        Model Model;
        int numberCollisions;


        public Lab06()
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
            Time.Initialize();
            InputManager.Initialize();
            // TODO: Add your initialization logic here
            random = new Random();
            transforms = new List<Transform>();
            rigidbodies = new List<Rigidbody>();
            Colliders = new List<Collider>();
            boxCollider = new BoxCollider();
            boxCollider.Size = 10;

            for (int i = 0; i < 2; i++)
            {
                Transform transform = new Transform();
                transform.LocalPosition += Vector3.Right * 5 * i; //avoid overlapping each sphere 
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

                transforms.Add(transform);
                Colliders.Add(sphereCollider);
                rigidbodies.Add(rigidbody);
            }

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
            Camera = new Camera();
            cameraTransform = new Transform();
            Camera.Transform = cameraTransform;
            cameraTransform.LocalPosition = Vector3.Backward * 25;

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
                    if (Colliders[i].Collides(Colliders[j], out normal)) { 
                     numberCollisions++;

                    Vector3 velocityNormal = Vector3.Dot(normal,
                        rigidbodies[i].Velocity - rigidbodies[j].Velocity) * -2
                           * normal * rigidbodies[i].Mass * rigidbodies[j].Mass;
                    rigidbodies[i].Impulse += velocityNormal / 2;
                    rigidbodies[j].Impulse += -velocityNormal / 2;
                    }
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
        foreach (Transform transform in transforms)
            Model.Draw(transform.World, Camera.View, Camera.Projection);
        base.Draw(gameTime);
        }
    }
}
