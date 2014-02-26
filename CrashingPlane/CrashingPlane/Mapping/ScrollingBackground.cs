using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CrashingPlane;

namespace CrashingPlane.Mapping
{
    class ScrollingBackground
    {
        Texture2D[] textures;
        int currentIndex;
        Vector2 position;
        float speedX;
        bool isScrolling;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="textures">A set of background textures. Must have the same size.</param>
        /// <param name="speedX">Speed of scrolling on X axis</param>
        public ScrollingBackground(Texture2D[] textures, float speedX, bool top)
        {
            this.textures = textures;
            this.speedX = speedX;
            currentIndex = 0;
            isScrolling = true;
            if (top)
            {
                // Textures shall fit screen top. (eg clouds)
                position.Y = 0;
            }
            else
            {
                // All textures shall fit in screen bottom.
                position.Y = GlobalHelper.GameHeight - textures[currentIndex].Height;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (isScrolling)
            {
                position.X += speedX * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Math.Abs(position.X) >= textures[currentIndex].Width)
                {
                    position.X = position.X % textures[currentIndex].Width;
                    currentIndex = (currentIndex + 1) % textures.Length;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, 1);
        }

        public void Draw(SpriteBatch spriteBatch, float alphaFrac)
        {
            // draw the current background
            spriteBatch.Draw(textures[currentIndex], position, Color.FromNonPremultiplied(255, 255, 255, (int)(255 * alphaFrac)));

            // draw the next background after the current one, to create the scrolling illusion
            spriteBatch.Draw(textures[(currentIndex + 1) % textures.Length],
                new Vector2(position.X + textures[currentIndex].Width, position.Y), Color.FromNonPremultiplied(255, 255, 255, (int)(255 * alphaFrac)));
        }

        public void Start()
        {
            isScrolling = true;
        }

        public void Stop()
        {
            isScrolling = false;
        }
    }
}
