using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrashingPlane.Screens
{
    class PausedScreen
    {
        Texture2D blank;
        SpriteFont pauseFont;

        const string PAUSE_TEXT = "GAME PAUSED";
        const float RECTANGLE_BORDER = 5;
        Vector2 position = new Vector2(1500, 170);

        GamePadState gps, lastGps;
#if WINDOWS
        KeyboardState kbs, lastKbs;
#endif

        public PausedScreen()
        {
            blank = ContentHolder.Blank;
            pauseFont = ContentHolder.MainFont;
        }

        public void Update(GameTime gameTime)
        {
            // check input for resume
#if WINDOWS
            lastKbs = kbs;
            kbs = Keyboard.GetState();
            if (kbs.IsKeyUp(Keys.Enter) && lastKbs.IsKeyDown(Keys.Enter))
            {
                GlobalHelper.CurrentGameState = GameState.Playing;
            }
#endif
            lastGps = gps;
            gps = GamePad.GetState((PlayerIndex)GlobalHelper.ActivePlayerIndex);
            if (gps.IsButtonUp(Buttons.Start) && lastGps.IsButtonDown(Buttons.Start))
            {
                // resume the game
                GlobalHelper.CurrentGameState = GameState.Playing;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw back rectangle first
            spriteBatch.Draw(blank, GetBackRectangle(), Color.Gray);

            // draw pause text
            spriteBatch.DrawString(pauseFont, PAUSE_TEXT, position, Color.White);
        }

        private Rectangle GetBackRectangle()
        {
            Vector2 textSize = pauseFont.MeasureString(PAUSE_TEXT);

            return new Rectangle((int)(position.X - RECTANGLE_BORDER), (int)(position.Y - RECTANGLE_BORDER),
                (int)(textSize.X + RECTANGLE_BORDER * 2), (int)(textSize.Y + RECTANGLE_BORDER * 2));
        }
    }
}
