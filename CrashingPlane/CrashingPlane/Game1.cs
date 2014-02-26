using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CrashingPlane.Screens;

namespace CrashingPlane
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont mainFont, debugFont;
        RenderTarget2D renderTarget;
        KeyboardState lastKbs, kbs;

        public World world;

        // Game screens. There could only be 1 instance each.
        LoadingScreen loadingScreen;
        PressStartScreen pressStartScreen;
        public GameplayScreen gameplayScreen;
        PausedScreen pausedScreen;
        GameOverScreen gameOverScreen;

        public Game1()
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
            // Startup graphics settings
            GlobalHelper.WindowWidth = GraphicsDevice.DisplayMode.Width;
            GlobalHelper.WindowHeight = GraphicsDevice.DisplayMode.Height;
            //GlobalHelper.WindowWidth = 640;
            //GlobalHelper.WindowHeight = 480;
            //graphics.ToggleFullScreen();
            // if it's in 4:3 but "widescreen" at the same time, just use a fake 16:9 resolution
            if (GraphicsDevice.DisplayMode.AspectRatio == (float)4 / 3 && GraphicsDevice.Adapter.IsWideScreen)
            {
                GlobalHelper.WindowWidth = 1280;    // do NOT use 1080p as Xbox won't automatically scale to 480p
                GlobalHelper.WindowHeight = 720;
            }
            // msaa settings is done when preparing
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            graphics.PreferredBackBufferWidth = GlobalHelper.WindowWidth;
            graphics.PreferredBackBufferHeight = GlobalHelper.WindowHeight;
            graphics.ApplyChanges();

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

            // Create render target
            renderTarget = new RenderTarget2D(GraphicsDevice, GlobalHelper.GameWidth, GlobalHelper.GameHeight, false, SurfaceFormat.Color, GraphicsDevice.PresentationParameters.DepthStencilFormat, GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);

            // Load essential content and create loading screen
            debugFont = Content.Load<SpriteFont>(@"fonts\debugFont");
            Texture2D loadingSpinTexture = Content.Load<Texture2D>(@"textures\titles\loadingSpin");
            ContentHolder.MainFont = Content.Load<SpriteFont>(@"fonts\mainFont");
            mainFont = ContentHolder.MainFont;
            loadingScreen = new LoadingScreen(mainFont, loadingSpinTexture);

            // Load all assets in another thread
            System.Threading.Thread loadingThread = new System.Threading.Thread(ThreadLoading);
            loadingThread.Start();
        }

        public void ThreadLoading()
        {
            ContentHolder.LoadAllContent(Content);
            world = new World();
            gameplayScreen = new GameplayScreen(this);
            pressStartScreen = new PressStartScreen(this);
            pausedScreen = new PausedScreen();
            gameOverScreen = new GameOverScreen(this);
            if (GlobalHelper.CurrentGameState == GameState.Loading) GlobalHelper.CurrentGameState = GameState.PressStart;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Fullscreen toggles
            lastKbs = kbs;
            kbs = Keyboard.GetState();
            if (kbs.IsKeyUp(Keys.F10) && lastKbs.IsKeyDown(Keys.F10))
                graphics.ToggleFullScreen();

            switch (GlobalHelper.CurrentGameState)
            {
                case GameState.Loading:
                    loadingScreen.Update(gameTime);
                    break;

                case GameState.PressStart:
                    pressStartScreen.Update(gameTime);
                    break;

                case GameState.Playing:
                    gameplayScreen.Update(gameTime);
                    break;

                case GameState.Paused:
                    pausedScreen.Update(gameTime);
                    break;

                case GameState.GameOver:
                    gameOverScreen.Update(gameTime);
                    break;
            }

            // update world
            if (GlobalHelper.CurrentGameState == GameState.PressStart || GlobalHelper.CurrentGameState == GameState.Playing)
                world.Update(gameTime); // we don't update world when paused, loading or gameover

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            bool isNative = GlobalHelper.IsRenderNative;

            // set render target
            if (!isNative)
            {
                GraphicsDevice.SetRenderTarget(renderTarget);
                GraphicsDevice.Clear(Color.Black);
            }

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // we always draw world first (it has nice backgrounds), but don't draw when loading
            if (GlobalHelper.CurrentGameState != GameState.Loading)
                world.Draw(spriteBatch);

            switch (GlobalHelper.CurrentGameState)
            {
                case GameState.Loading:
                    loadingScreen.Draw(spriteBatch);
                    break;

                case GameState.PressStart:
                    pressStartScreen.Draw(spriteBatch);
                    break;

                case GameState.Playing:
                    gameplayScreen.Draw(spriteBatch);
                    break;

                case GameState.Paused:
                    pausedScreen.Draw(spriteBatch);
                    break;

                case GameState.GameOver:
                    gameOverScreen.Draw(spriteBatch);
                    break;
            }

            // output debug messages
            if (GlobalHelper.CurrentGameState != GameState.Loading && GlobalHelper.DebugMode)
                spriteBatch.DrawString(debugFont,
                    "In-game time: " + world.InGameTime +
                    "\nThis session: " + world.TimeSpentFlying +
                    "\nBrightness: " + world.inGameTimer.Brightness + 
                    "\nMoonRotation: " + world.inGameTimer.MoonRotation + 
                    "\nChopper Y: " + (world.Player == null ? "null" : world.Player.Y.ToString()) +
                    "\nChopper V_Y: " + (world.Player == null ? "null" : world.Player.VelocityY.ToString()) +
                    "\nChopper A_Y: " + (world.Player == null ? "null" : world.Player.Acceleration.ToString()),
                    Vector2.Zero,
                    Color.White);

            spriteBatch.End();

            if (!isNative)
            {
                // set to back buffer
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, GraphicsDevice.RasterizerState, null);
                spriteBatch.Draw(renderTarget, new Rectangle((int)GlobalHelper.RenderOrigin.X, (int)GlobalHelper.RenderOrigin.Y,
                    GlobalHelper.RenderWidth, GlobalHelper.RenderHeight), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 4;
        }
    }
}
