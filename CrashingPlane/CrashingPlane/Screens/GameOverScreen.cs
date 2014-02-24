using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrashingPlane.Screens
{
    class GameOverScreen
    {
        GameplayScreen gameplayScreen;
        World world;

        TimeSpan timeSpan;
        string scoreText;
        Vector2 position;
        const float RECTANGLE_BORDER = 5;

        SpriteFont font;
        Texture2D blank;

        GamePadState gps, lastGps;
#if WINDOWS
        KeyboardState kbs, lastKbs;
#endif

        public GameOverScreen(Game1 game)
        {
            gameplayScreen = game.gameplayScreen;
            world = game.world;
            font = ContentHolder.MainFont;
            blank = ContentHolder.Blank;
        }

        public void Update(GameTime gameTime)
        {
            lastGps = gps;
            gps = GamePad.GetState((PlayerIndex)GlobalHelper.ActivePlayerIndex);

#if WINDOWS
            lastKbs = kbs;
            kbs = Keyboard.GetState();
            if (kbs.IsKeyUp(Keys.Enter) && lastKbs.IsKeyDown(Keys.Enter))
            {
                GlobalHelper.CurrentGameState = GameState.Playing;
                gameplayScreen.RestartGame();
            }
#endif

            // Get the time span from in-game timer
            timeSpan = world.TimeSpentFlying;
            // Construct score string
            scoreText = "Your helicopter crashed after flying for\n"
                + timeSpan.Days + " days " + timeSpan.Hours + " hours " + timeSpan.Minutes + " minutes (in-game).\n"
                + "Press START to retry.";

            if (gps.IsButtonUp(Buttons.Start) && lastGps.IsButtonDown(Buttons.Start))
            {
                GlobalHelper.CurrentGameState = GameState.Playing;
                gameplayScreen.RestartGame();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Update position to screen center
            if (scoreText != null)
            {
                position = new Vector2((GlobalHelper.GameWidth - font.MeasureString(scoreText).X) / 2,
                    (GlobalHelper.GameHeight - font.MeasureString(scoreText).Y) / 2);

                spriteBatch.Draw(blank, GetBackRectangle(), Color.Gray);
                spriteBatch.DrawString(font, scoreText, position, Color.Blue);
            }
        }

        private Rectangle GetBackRectangle()
        {
            Vector2 textSize = font.MeasureString(scoreText);

            return new Rectangle((int)(position.X - RECTANGLE_BORDER), (int)(position.Y - RECTANGLE_BORDER),
                (int)(textSize.X + RECTANGLE_BORDER * 2), (int)(textSize.Y + RECTANGLE_BORDER * 2));
        }
    }
}
