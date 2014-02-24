using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrashingPlane.Screens
{
    class PressStartScreen
    {
        GameplayScreen gameplayScreen; // reference to gameplayScreen

        Texture2D titleTexture, pressStartTexture;
        GamePadState[] gamePadStates = new GamePadState[4];
        GamePadState[] lastGamePadStates = new GamePadState[4];
#if WINDOWS
        KeyboardState kbs, lastKbs;
#endif

        float rotation = 0; // rotation for pressStart texture
        const float MAX_ROTATION = 0.14f;

        float scale = 1;    // scale for title texture
        const float MAX_SCALE_OFFSET = 0.1f;

        public PressStartScreen(Game1 game)
        {
            titleTexture = ContentHolder.TitleTexture;
            pressStartTexture = ContentHolder.PressStartTexture;
            gameplayScreen = game.gameplayScreen;
        }

        public void Update(GameTime gameTime)
        {
            double time = gameTime.TotalGameTime.TotalSeconds;

            // Update animations
            rotation = (float)Math.Sin(time * 5) * MAX_ROTATION;
            scale = 1 + (float)Math.Sin(time * 8) * MAX_SCALE_OFFSET;

            // Update game pad states
            for (int i = 0; i < 4; i++)
            {
                lastGamePadStates[i] = gamePadStates[i];
                gamePadStates[i] = GamePad.GetState((PlayerIndex)i);
            }

#if WINDOWS
            lastKbs = kbs;
            kbs = Keyboard.GetState();
            if (kbs.IsKeyUp(Keys.Enter) && lastKbs.IsKeyDown(Keys.Enter))
            {
                GlobalHelper.ActivePlayerIndex = PlayerIndex.One;
            }
#endif

            if (GlobalHelper.ActivePlayerIndex == null)
            {
                // set active player index to the one controller that pressed start
                for (int i = 0; i < 4; i++)
                {
                    if (gamePadStates[i].IsButtonUp(Buttons.Start) && lastGamePadStates[i].IsButtonDown(Buttons.Start))
                    {
                        GlobalHelper.ActivePlayerIndex = (PlayerIndex)i;
                        break;
                    }
                }
            }
            else
            {
                // start the game
                GlobalHelper.CurrentGameState = GameState.Playing;
                gameplayScreen.RestartGame();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleTexture, new Vector2(GlobalHelper.GameWidth / 2, GlobalHelper.GameHeight / 2 - 50), 
                null, Color.White, rotation, 
                new Vector2(titleTexture.Width / 2, titleTexture.Height / 2), 
                1f, SpriteEffects.None, 0.5f);

            spriteBatch.Draw(pressStartTexture, new Vector2(GlobalHelper.GameWidth / 2, GlobalHelper.GameHeight / 2 + 100),
                null, Color.White, 0,
                new Vector2(pressStartTexture.Width / 2, pressStartTexture.Height / 2),
                scale, SpriteEffects.None, 1f);
        }
    }
}
