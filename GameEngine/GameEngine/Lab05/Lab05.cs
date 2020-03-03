using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace Lab05
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab05 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model model;
        Transform parentTransform;
        Transform childTransform;
        Transform cameraTransform;
        Texture2D Texture;
        Camera camera;
        Effect effect;
        int currentTechnique = 0;

        public Lab05()
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
            Texture = Content.Load<Texture2D>("Square");
            effect = Content.Load<Effect>("SimpleShading");

            parentTransform = new Transform();
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 2;
            camera = new Camera();
            camera.Transform = cameraTransform;


            foreach (ModelMesh mesh in model.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

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

            // TODO: Add your update logic here
            Time.Update(gameTime);
            InputManager.Update();

          

            if (InputManager.IsKeyPressed(Keys.Tab))
            {
                if (currentTechnique != 3)
                {
                    currentTechnique++;
                }
                else
                {
                    currentTechnique = 0;
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

            Matrix view = camera.View;
            Matrix projection = camera.Projection;

            effect.CurrentTechnique = effect.Techniques[currentTechnique]; //"0" is the first technique
            effect.Parameters["World"].SetValue(parentTransform.World);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
                                                                     Vector3.Right * 5);
            effect.Parameters["CameraPosition"].SetValue(cameraTransform.Position);
            effect.Parameters["Shininess"].SetValue(20f);
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.5f, 0.5f, 0.5f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0.9f, 0.9f, 0.9f));
            effect.Parameters["DiffuseTexture"].SetValue(Texture);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (ModelMesh mesh in model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        GraphicsDevice.Indices = part.IndexBuffer;
                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                    }
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
