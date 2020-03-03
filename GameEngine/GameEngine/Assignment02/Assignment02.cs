using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Runtime.InteropServices;
using System;
using System.Windows.Input;

namespace Assignment02
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Assignment02 : Game
    {
      /*  GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Model sun;
        Transform sunTransform;
        Model mercury;
        Transform mercuryTransform;
        Model earth;
        Transform earthTransform;
        Model moon;
        Transform moonTransform;
        Model player;
        Transform playerTransform;
        Model ground;
        Transform groundTransform;
        Transform cameraTransform;
        Transform cameraParentTransform;
        Transform cameraTransform2;
        Camera camera;
        Camera camera2;
        Effect effect;
        Texture2D Texture;
        Effect effect2;
        Effect effect3;
        bool isNotFirstPerson = false;
        float rotateSpeed = 1.0f;
        [DllImport("user32.dll")]  static extern bool GetCursorPos(out Point lpPoint);
        static int MouseX;
        static int MouseY;

        Vector3 sunPosition = new Vector3(0, 0, 0);
        Vector3 sunRotation = new Vector3(0, 0, 0);
        Vector3 sunScale = new Vector3(5, 5, 5);
        Vector3 earthPosition = new Vector3(-10, 0, 0);
        Vector3 earthRotation = new Vector3(0, 0, 0);
        Vector3 earthScale = new Vector3(3, 3, 3);
        Vector3 mercuryPosition = new Vector3(10, 0, 0);
        Vector3 mercuryRotation = new Vector3(0, 0, 0);
        Vector3 mercuryScale = new Vector3(2, 2, 2);
        Vector3 moonPosition = new Vector3(-10, 0, 0);
        Vector3 moonRotation = new Vector3(0, 0, 0);
        Vector3 moonScale = new Vector3(1, 1, 1);

        public Assignment02()
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
            InputManager.Initialize();
            Time.Initialize();
            base.Initialize();
            this.IsMouseVisible = true;
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

            sun = Content.Load<Model>("Sphere");
            sunTransform = new Transform();
            sunTransform.LocalScale = new Vector3(5, 5, 5);


            earth = Content.Load<Model>("Sphere");
            earthTransform = new Transform();
            earthTransform.LocalScale = new Vector3(0.6f, 0.6f, 0.6f);
            earthTransform.LocalPosition = new Vector3(7, 0, 0);
            earthTransform.Parent = sunTransform;

            mercury = Content.Load<Model>("Sphere");
            mercuryTransform = new Transform();
            mercuryTransform.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
            mercuryTransform.LocalPosition = new Vector3(3, 0, 0);
            mercuryTransform.Parent = sunTransform;

            moon = Content.Load<Model>("Sphere");
            moonTransform = new Transform();
            moonTransform.LocalScale = new Vector3(0.33f, 0.33f, 0.33f);
            moonTransform.LocalPosition = new Vector3(earthTransform.LocalPosition.X / 4, 0, 0);
            moonTransform.Parent = earthTransform;


            cameraTransform = new Transform();
            cameraTransform.LocalPosition = new Vector3( 0, -49, 0);
            camera = new Camera();
            camera.Transform = cameraTransform;
            camera.NearPlane = 0.1f;
            camera.FarPlane = 1000f;
            cameraTransform.Parent = cameraParentTransform;

            cameraParentTransform = new Transform();
            cameraParentTransform.LocalPosition = cameraTransform.Position;


            cameraTransform2 = new Transform();
            cameraTransform2.LocalPosition = new Vector3(0, 20, 70);
            camera2 = new Camera();
            camera2.Transform = cameraTransform2;
            camera2.NearPlane = 0.1f;
            camera2.FarPlane = 1000f;
           

            ground = Content.Load<Model>("Plane");
            groundTransform = new Transform();
            groundTransform.LocalScale = new Vector3(25, 1, 25);
            groundTransform.LocalPosition = new Vector3(0, -50, 0);

            player = Content.Load<Model>("CubeRig");
            playerTransform = new Transform();
            playerTransform.LocalScale = new Vector3(0.05f, 0.05f, 0.05f);
           // playerTransform.LocalPosition = new Vector3(0, -49, 0);
           

            Texture = Content.Load<Texture2D>("Square");
            effect = Content.Load<Effect>("SimpleShading");
            effect2 = Content.Load<Effect>("SimpleShading");
            effect3 = Content.Load<Effect>("SimpleShading");


            foreach (ModelMesh mesh in sun.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            foreach (ModelMesh mesh in moon.Meshes)
                foreach (BasicEffect effect2 in mesh.Effects)
                    effect2.EnableDefaultLighting();

            foreach (ModelMesh mesh in mercury.Meshes)
                foreach (BasicEffect effect3 in mesh.Effects)
                    effect3.EnableDefaultLighting();

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
           
            var mouseState = Mouse.GetState();
            
            // TODO: Add your update logic here
            InputManager.Update();
            Time.Update(gameTime);
            earthTransform.Rotate(Vector3.Up, -Time.ElapsedGameTime * 2 * rotateSpeed);
            sunTransform.Rotate(Vector3.Up, -Time.ElapsedGameTime * rotateSpeed);
            playerTransform.LocalPosition = cameraTransform.Position;

            if (mouseState.X < 800 && mouseState.Y < 480)
            {

                if (isNotFirstPerson)
                {
                    cameraTransform.Rotate(Vector3.Right, InputManager.IsMouseMovedY() * -Time.ElapsedGameTime);
                    cameraTransform.Rotate(Vector3.Up, InputManager.IsMouseMovedX() * -Time.ElapsedGameTime);
                }
            }
            if (InputManager.IsKeyPressed(Keys.Tab))
            {
                isNotFirstPerson = !isNotFirstPerson;

            }
            if (InputManager.IsMousePressed())
            {
                Console.WriteLine("Bang");
                cameraTransform.LocalPosition += camera.Transform.Forward / 4;

            }

            if (InputManager.IsKeyDown(Keys.D))
            {
                cameraTransform.LocalPosition -= camera.Transform.Left; 
              

            }
            if (InputManager.IsKeyDown(Keys.A))
            {
                cameraTransform.LocalPosition -= camera.Transform.Right;
               

            }
            if (InputManager.IsKeyDown(Keys.W))
            {
                cameraTransform.Position += camera.Transform.Forward;
               

            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                cameraTransform.Position += camera.Transform.Backward;
  
            }
            if (InputManager.IsKeyDown(Keys.Left))
            {
                cameraTransform.Rotate(Vector3.Down, -Time.ElapsedGameTime * 2);

            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                cameraTransform.Rotate(Vector3.Up,  -Time.ElapsedGameTime * 2);

            }
            if (InputManager.IsKeyDown(Keys.B))
            {
                rotateSpeed *= 1.01f;

            }
            if (InputManager.IsKeyDown(Keys.V))
            {
                rotateSpeed *= 0.99f;

            }
            if (InputManager.IsKeyDown(Keys.M))
            {
                if (camera.FieldOfView > 1)
                {
                    camera.FieldOfView -= Time.ElapsedGameTime / 2;
                }

            }
            if (InputManager.IsKeyDown(Keys.N))
            {
                if (camera.FieldOfView < 3)
                {
                    camera.FieldOfView += Time.ElapsedGameTime / 2;
                }

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

            //sun shader
            if (!isNotFirstPerson)
            {
                effect.CurrentTechnique = effect.Techniques[0]; //"0" is the first technique
                effect.Parameters["World"].SetValue(sunTransform.World);
                effect.Parameters["View"].SetValue(camera2.View);
                effect.Parameters["Projection"].SetValue(camera2.Projection);
                effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
                                                                         Vector3.Right * 5);
                effect.Parameters["CameraPosition"].SetValue(cameraTransform2.Position);
                effect.Parameters["Shininess"].SetValue(2f);
                effect.Parameters["AmbientColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect.Parameters["DiffuseColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect.Parameters["SpecularColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect.Parameters["DiffuseTexture"].SetValue(Texture);

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    foreach (ModelMesh mesh in sun.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                            GraphicsDevice.Indices = part.IndexBuffer;
                            GraphicsDevice.DrawIndexedPrimitives(
                                PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                        }
                }

                //moon shader
                effect2.CurrentTechnique = effect2.Techniques[0]; //"0" is the first technique
                effect2.Parameters["World"].SetValue(moonTransform.World);
                effect2.Parameters["View"].SetValue(camera2.View);
                effect2.Parameters["Projection"].SetValue(camera2.Projection);
                effect2.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
                                                                         Vector3.Right * 5);
                effect2.Parameters["CameraPosition"].SetValue(cameraTransform2.Position);
                effect2.Parameters["Shininess"].SetValue(20f);
                effect2.Parameters["AmbientColor"].SetValue(new Vector3(0.8f, 0.8f, 0.8f));
                effect2.Parameters["DiffuseColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect2.Parameters["SpecularColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect2.Parameters["DiffuseTexture"].SetValue(Texture);

                foreach (EffectPass pass in effect2.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    foreach (ModelMesh mesh in moon.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                            GraphicsDevice.Indices = part.IndexBuffer;
                            GraphicsDevice.DrawIndexedPrimitives(
                                PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                        }
                }

                //mercury shader
                effect3.CurrentTechnique = effect3.Techniques[0]; //"0" is the first technique
                effect3.Parameters["World"].SetValue(mercuryTransform.World);
                effect3.Parameters["View"].SetValue(camera2.View);
                effect3.Parameters["Projection"].SetValue(camera2.Projection);
                effect3.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
                                                                         Vector3.Right * 5);
                effect3.Parameters["CameraPosition"].SetValue(cameraTransform2.Position);
                effect3.Parameters["Shininess"].SetValue(20f);
                effect3.Parameters["AmbientColor"].SetValue(new Vector3(0.9f, 0.1f, 0.25f));
                effect3.Parameters["DiffuseColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect3.Parameters["SpecularColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect3.Parameters["DiffuseTexture"].SetValue(Texture);

                foreach (EffectPass pass in effect3.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    foreach (ModelMesh mesh in mercury.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                            GraphicsDevice.Indices = part.IndexBuffer;
                            GraphicsDevice.DrawIndexedPrimitives(
                                PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                        }
                }
                // TODO: Add your drawing code here

                earth.Draw(earthTransform.World, camera2.View, camera2.Projection);
                ground.Draw(groundTransform.World, camera2.View, camera2.Projection);
                player.Draw(playerTransform.World, camera2.View, camera2.Projection);
            }

            else
            {
                effect.CurrentTechnique = effect.Techniques[0]; //"0" is the first technique
                effect.Parameters["World"].SetValue(sunTransform.World);
                effect.Parameters["View"].SetValue(camera.View);
                effect.Parameters["Projection"].SetValue(camera.Projection);
                effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
                                                                         Vector3.Right * 5);
                effect.Parameters["CameraPosition"].SetValue(cameraTransform.Position);
                effect.Parameters["Shininess"].SetValue(2f);
                effect.Parameters["AmbientColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect.Parameters["DiffuseColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect.Parameters["SpecularColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect.Parameters["DiffuseTexture"].SetValue(Texture);

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    foreach (ModelMesh mesh in sun.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                            GraphicsDevice.Indices = part.IndexBuffer;
                            GraphicsDevice.DrawIndexedPrimitives(
                                PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                        }
                }

                //moon shader
                effect2.CurrentTechnique = effect2.Techniques[0]; //"0" is the first technique
                effect2.Parameters["World"].SetValue(moonTransform.World);
                effect2.Parameters["View"].SetValue(camera.View);
                effect2.Parameters["Projection"].SetValue(camera.Projection);
                effect2.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
                                                                         Vector3.Right * 5);
                effect2.Parameters["CameraPosition"].SetValue(cameraTransform.Position);
                effect2.Parameters["Shininess"].SetValue(20f);
                effect2.Parameters["AmbientColor"].SetValue(new Vector3(0.8f, 0.8f, 0.8f));
                effect2.Parameters["DiffuseColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect2.Parameters["SpecularColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect2.Parameters["DiffuseTexture"].SetValue(Texture);

                foreach (EffectPass pass in effect2.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    foreach (ModelMesh mesh in moon.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                            GraphicsDevice.Indices = part.IndexBuffer;
                            GraphicsDevice.DrawIndexedPrimitives(
                                PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                        }
                }

                //mercury shader
                effect3.CurrentTechnique = effect3.Techniques[0]; //"0" is the first technique
                effect3.Parameters["World"].SetValue(mercuryTransform.World);
                effect3.Parameters["View"].SetValue(camera.View);
                effect3.Parameters["Projection"].SetValue(camera.Projection);
                effect3.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
                                                                         Vector3.Right * 5);
                effect3.Parameters["CameraPosition"].SetValue(cameraTransform.Position);
                effect3.Parameters["Shininess"].SetValue(20f);
                effect3.Parameters["AmbientColor"].SetValue(new Vector3(0.9f, 0.1f, 0.25f));
                effect3.Parameters["DiffuseColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect3.Parameters["SpecularColor"].SetValue(new Vector3(1.0f, 1.0f, 0.5f));
                effect3.Parameters["DiffuseTexture"].SetValue(Texture);

                foreach (EffectPass pass in effect3.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    foreach (ModelMesh mesh in mercury.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                            GraphicsDevice.Indices = part.IndexBuffer;
                            GraphicsDevice.DrawIndexedPrimitives(
                                PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                        }
                }
                // TODO: Add your drawing code here

                earth.Draw(earthTransform.World, camera.View, camera.Projection);
                ground.Draw(groundTransform.World, camera.View, camera.Projection);
            }
           
            base.Draw(gameTime);
        }*/
    }
}
