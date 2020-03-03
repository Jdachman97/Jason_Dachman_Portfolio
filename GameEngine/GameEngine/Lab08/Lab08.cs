using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Threading;
using System.Collections.Generic;
using CPI311.GameEngine;
using CPI311.GameEngine.Rendering;

namespace Lab08
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab08 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SoundEffect soundEffect;
        SoundEffect backgroundMusic;
        Model model;
        Camera camera, topDownCamera;
        List<Transform> transforms;
        List<Collider> Colliders;
        List<Camera> cameras;
        Light Light;
        Transform LightTransform;
        String filename;
        Texture2D texture;
        List<Renderer> renderers;
        List<Rigidbody> rigidbodies;
      //  ScreenManager screenManager;
        Effect effect;
        Ray ray;




        public Lab08()
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
            this.IsMouseVisible = true;
            model = Content.Load<Model>("Sphere");
            ScreenManager.Setup(true, 1920, 1080);
            texture = Content.Load<Texture2D>("Square");

            transforms = new List<Transform>();
            rigidbodies = new List<Rigidbody>();
            Colliders = new List<Collider>();
            renderers = new List<Renderer>();
           

          //  camera = new Camera();
           // topDownCamera = new Camera();
            Light = new Light();
            LightTransform = new Transform();
            Light.Transform = LightTransform;
            cameras = new List<Camera>();

            foreach (ModelMesh mesh in model.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

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
            soundEffect = Content.Load<SoundEffect>("Gun");
            effect = Content.Load<Effect>("SimpleShading2");

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5;
            camera.Position = new Vector2(0f, 0f);
            camera.Size = new Vector2(0.5f, 1f);
            camera.AspectRatio = camera.Viewport.AspectRatio;

            topDownCamera = new Camera();
            topDownCamera.Transform = new Transform();
            topDownCamera.Transform.LocalPosition = Vector3.Up * 10;
            topDownCamera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            topDownCamera.Position = new Vector2(0.5f, 0f);
            topDownCamera.Size = new Vector2(0.5f, 1f);
            topDownCamera.AspectRatio = topDownCamera.Viewport.AspectRatio;

            cameras.Add(topDownCamera);
            cameras.Add(camera);

            AddSphere();
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
            Ray ray = camera.ScreenPointToWorldRay (InputManager.GetMousePosition());
            Ray ray2 = topDownCamera.ScreenPointToWorldRay(InputManager.GetMousePosition());
            float nearest = Single.MaxValue; // Start with highest value
            float? p;
            Collider target = null; // Assume no intersection
            foreach (Collider collider in Colliders)
                if ((p = collider.Intersects(ray)) != null)
                {
                    float q = (float)p;
                    if (q < nearest)
                        nearest = q;
                    target = collider;
                }
           else if ((p = collider.Intersects(ray2)) != null)
            {
                float q = (float)p;
                if (q < nearest)
                    nearest = q;
                target = collider;
            }


            foreach (Collider collider in Colliders)
            {

                if (collider.Intersects(ray) != null || collider.Intersects(ray2) != null)
                {
                    
                    effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                    (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                                                 Color.Blue.ToVector3();
                    if (InputManager.IsMousePressed(0))
                    {
                        SoundEffectInstance instance = soundEffect.CreateInstance();

                        instance.Play();

                    }
                }
                else 
                {
                    
                    effect.Parameters["DiffuseColor"].SetValue(Color.Blue.ToVector3());
                    (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                                                 Color.Red.ToVector3();
                }


             /*   if (collider.Intersects(ray2) != null)
                {

                    effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                    (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                                                 Color.Blue.ToVector3();
                    if (InputManager.IsMousePressed(0))
                    {
                        SoundEffectInstance instance = soundEffect.CreateInstance();

                        instance.Play();

                    }
                }
                else
                {

                    effect.Parameters["DiffuseColor"].SetValue(Color.Blue.ToVector3());
                    (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                                                 Color.Red.ToVector3();
                }*/
            }


            // TODO: Add your update logic here
            Time.Update(gameTime);
            InputManager.Update();

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
            foreach (Camera camera in cameras)
            {
                GraphicsDevice.DepthStencilState = new DepthStencilState();
                GraphicsDevice.Viewport = camera.Viewport;
                Matrix view = camera.View;
                Matrix projection = camera.Projection;

                effect.CurrentTechnique = effect.Techniques[1];
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                effect.Parameters["LightPosition"].SetValue(camera.Transform.Position);
                effect.Parameters["Shininess"].SetValue(20.0f);

                effect.Parameters["AmbientColor"].SetValue(Color.Blue.ToVector3());

                effect.Parameters["SpecularColor"].SetValue(Color.Blue.ToVector3());

                effect.Parameters["DiffuseTexture"].SetValue(texture);

                foreach (Transform transform in transforms)
                {
                    effect.Parameters["World"].SetValue(transform.World);
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        foreach (ModelMesh mesh in model.Meshes)
                            foreach (ModelMeshPart part in mesh.MeshParts)
                            {
                                GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                                GraphicsDevice.Indices = part.IndexBuffer;
                                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                            }
                    }
                }

            }
                base.Draw(gameTime);
        }

        private void AddSphere()
        {

            Transform transform = new Transform();
           // transform.LocalPosition += Vector3.Right * 5; //avoid overlapping each sphere 
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = transform;

            rigidbody.Mass = 1;

            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1.0f * transform.LocalScale.Y;
            sphereCollider.Transform = transform;

            Light light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalScale = Vector3.One * 2f;
            float speed = rigidbody.Velocity.Length();
            float speedValue = MathHelper.Clamp(speed / 20f, 0, 1);
            //  Console.WriteLine(speedValue);
         //   light.Ambient = new Color(0.9f, 0.9f, 0.9f, 1.0f);
           // light.Diffuse = new Color(0.9f, 0.9f, 0.9f, 1.0f);
           // light.Specular = new Color(0.9f, 0.9f, 0.9f, 1.0f);

            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(model, transform, camera, Content,
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
