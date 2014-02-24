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
        Texture2D backgroundTexture;
        float scrollSpeed;
        float x;
		bool isScrolling;

        public ScrollingBackground(Texture2D backgroundTexture, float scrollSpeed)
        {
            this.backgroundTexture = backgroundTexture;
            this.scrollSpeed = scrollSpeed;
			this.isScrolling = true;
        }

        public void Update(GameTime gameTime)
        {
			if (isScrolling)
			{
				x += scrollSpeed;
				if (backgroundTexture.Width - x < GlobalHelper.GameWidth)
					x = 0;
			}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, GlobalHelper.GameHeight - GetSourceRectangle().Height, 
                GetSourceRectangle().Width, GetSourceRectangle().Height), GetSourceRectangle(), Color.White);
        }

        private Rectangle GetSourceRectangle()
        {
            return new Rectangle((int)x, 0, GlobalHelper.GameWidth, backgroundTexture.Height);
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
