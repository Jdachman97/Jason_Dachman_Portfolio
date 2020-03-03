using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
using System;

namespace lab11
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class lab11 : Game
    {

        public class Scene
        {
            public delegate void CallMethod();
            public CallMethod Update;
            public CallMethod Draw;
            public Scene(CallMethod update, CallMethod draw)
            { Update = update; Draw = draw; }
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        SpriteFont font;
        Color background = Color.Blue;
       // Button exitButton;
        Dictionary<String, Scene> scenes;
        Scene currentScene;
        List<GUIElement> guiElements;


        public lab11()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            scenes = new Dictionary<string, Scene>();
            guiElements = new List<GUIElement>();
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

            texture = Content.Load<Texture2D>("Square");
            font = Content.Load<SpriteFont>("Font");

            GUIGroup group = new GUIGroup();

            Button exitButton = new Button();
            exitButton.Texture = texture;
            exitButton.Text = "Exit";
            exitButton.Bounds = new Rectangle(50, 50, 300, 20);

            exitButton.Action += ExitGame;
            group.Children.Add(exitButton);

            CheckBox optionBox = new CheckBox();
            optionBox.Texture = texture;
            optionBox.box = texture;
            optionBox.Bounds = new Rectangle(50, 75, 300, 20);
            optionBox.Text = "Full Screen";
            optionBox.Action += makeFullScreen;
            group.Children.Add(optionBox);
            guiElements.Add(group);


            scenes.Add("Menu", new Scene(MainMenuUpdate, MainMenuDraw));
            scenes.Add("Play", new Scene(PlayUpdate, PlayDraw));
            currentScene = scenes["Menu"];




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
           // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
             //   Exit();

            // TODO: Add your update logic here

            Time.Update(gameTime);
            InputManager.Update();
           // exitButton.Update();
            currentScene.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);

            // TODO: Add your drawing code here
            currentScene.Draw();

            spriteBatch.Begin();
           // exitButton.Draw(spriteBatch, font);
            spriteBatch.End();


            base.Draw(gameTime);
        }

        void ExitGame(GUIElement element)
        {
            currentScene = scenes["Play"];
            background = (background == Color.White ? Color.Blue : Color.White);
        }

        void MainMenuUpdate()
        {
            foreach (GUIElement element in guiElements)
                element.Update();
        }
        void MainMenuDraw()
        {
            spriteBatch.Begin();
            foreach (GUIElement element in guiElements)
                element.Draw(spriteBatch, font);
            spriteBatch.End();
        }
        void PlayUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
                currentScene = scenes["Menu"];
        }
        void PlayDraw()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Play Mode! Press \"Esc\" to go back", Vector2.Zero, Color.Black);
            spriteBatch.End();
        }

        void makeFullScreen(GUIElement element)
        {
            ScreenManager.Setup(!ScreenManager.IsFullScreen, ScreenManager.Width + 1, ScreenManager.Height + 1);
        }

    }
}
