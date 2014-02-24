using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrashingPlane.Screens
{
    public class GameplayScreen
    {
        World world;    // reference to game world
        GamePadState gps, lastGps;
#if WINDOWS
        KeyboardState kbs, lastKbs;
#endif

        int countdownNum = 3;  // a countdown before game starts. 0 = started
        int countdownTime = 0;
        Vector2 countdownPos;
        const float RECTANGLE_BORDER = 5;

        Texture2D blank;
        SpriteFont countdownFont;

        public GameplayScreen(Game1 game)
        {
            world = game.world;
            countdownFont = ContentHolder.BigFont;
            blank = ContentHolder.Blank;
        }

        public void Update(GameTime gameTime)
        {
            // update countdown time
            if (countdownNum != 0)
            {
                world.Wait(500);
                countdownTime += gameTime.ElapsedGameTime.Milliseconds;
                if (countdownTime > 1000)
                {
                    countdownTime -= 1000;
                    countdownNum--;
                }
            }

            // Update gamepad states
            lastGps = gps;
            gps = GamePad.GetState((PlayerIndex)GlobalHelper.ActivePlayerIndex);
            
#if WINDOWS
            lastKbs = kbs;
            kbs = Keyboard.GetState();
            if (kbs.IsKeyUp(Keys.Enter) && lastKbs.IsKeyDown(Keys.Enter))
            {
                GlobalHelper.CurrentGameState = GameState.Paused;
            }
            if (kbs.IsKeyDown(Keys.Space))
            {
                world.ApplyForceToPlayer();
            }
#endif

            // pause game control
            if (gps.IsButtonUp(Buttons.Start) && lastGps.IsButtonDown(Buttons.Start))
            {
                GlobalHelper.CurrentGameState = GameState.Paused;
            }

            // player chopper control
            if (gps.IsButtonDown(Buttons.A))
            {
                world.ApplyForceToPlayer();
            }

            // Finally, if it is end of gameplay, change state to game over
            if (world.EndOfGameplay)
            {
                GlobalHelper.CurrentGameState = GameState.GameOver;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw countdown text and back rectangle if its not 0
            if (countdownNum != 0)
            {
                // find center of screen
                countdownPos = new Vector2((GlobalHelper.GameWidth - countdownFont.MeasureString(countdownNum.ToString()).X) / 2,
                    (GlobalHelper.GameHeight - countdownFont.MeasureString(countdownNum.ToString()).Y) / 2);

                //spriteBatch.Draw(blank, GetCountdownBackRectangle(), Color.Gray);
                spriteBatch.DrawString(countdownFont, countdownNum.ToString(), countdownPos, Color.Red);
            }
        }

        /// <summary>
        /// Muse be called when game state is changed from PressStart or from GameOver to Playing.
        /// GameplayScreen WILL NOT CALL BY ITSELF.
        /// </summary>
        public void RestartGame()
        {
            world.ResetEntities();
            countdownNum = 3;
            countdownTime = 0;
        }

        private Rectangle GetCountdownBackRectangle()
        {
            Vector2 textSize = countdownFont.MeasureString(countdownNum.ToString());

            return new Rectangle((int)(countdownPos.X - RECTANGLE_BORDER), (int)(countdownPos.Y - RECTANGLE_BORDER),
                (int)(textSize.X + RECTANGLE_BORDER * 2), (int)(textSize.Y + RECTANGLE_BORDER * 2));
        }

    }
}
