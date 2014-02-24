using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrashingPlane.Screens
{
    class LoadingScreen
    {
        SpriteFont font;
        Texture2D spinTexture;
        StringBuilder textBuilder;
        int dotIndex = 0;
        int timeSinceLastDot = 0;
        float rotation = 0;
        string lastStatus;  // to check if status has changed

        Vector2 screenCenter;

        const int MS_PER_DOT = 100;

        public LoadingScreen(SpriteFont font, Texture2D spinTexture)
        {
            this.font = font;
            this.spinTexture = spinTexture;
            textBuilder = new StringBuilder(ContentHolder.StatusText);
            screenCenter = new Vector2(GlobalHelper.GameWidth / 2, GlobalHelper.GameHeight / 2);
        }

        public void Update(GameTime gameTime)
        {
            // Update loading text
            if (lastStatus != null && lastStatus != ContentHolder.StatusText) dotIndex = 0;
            lastStatus = ContentHolder.StatusText;
            
            timeSinceLastDot += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastDot > MS_PER_DOT)
            {
                timeSinceLastDot -= MS_PER_DOT;
                textBuilder = new StringBuilder(ContentHolder.StatusText);
                dotIndex = (dotIndex + 1) % textBuilder.Length;
                textBuilder[dotIndex] = '_';
            }

            // Update spin rotation
            rotation += 0.05f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw loading text
            Vector2 fontOrigin = font.MeasureString(textBuilder) / 2;
            spriteBatch.DrawString(font, textBuilder, screenCenter - new Vector2(0, 40), Color.White, 0,
                fontOrigin, 1f, SpriteEffects.None, 1f);

            // Draw spinning texture
            // ITS COLOR RED ATM
            Vector2 spinOrigin = new Vector2(spinTexture.Width / 2, spinTexture.Height / 2);
            spriteBatch.Draw(spinTexture, screenCenter + new Vector2(0, 40),
                null, Color.Red, rotation, spinOrigin, 1f, SpriteEffects.None, 0f);
        }
    }
}
