using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrashingPlane.Mapping
{
    class SkyBackground
    {
        Texture2D moonTexture, sunTexture;
        Texture2D nightTexture, dayTexture;
        InGameTimer inGameTimer;    // reference to in-game timer

        const float RADIUS = 950;
        Vector2 circleOrigin;
        Vector2 moonPos, sunPos;

        public SkyBackground(InGameTimer inGameTimer)
        {
            this.inGameTimer = inGameTimer;
            
            nightTexture = ContentHolder.NightSkyTexture;
            dayTexture = ContentHolder.DaySkyTexture;
            sunTexture = ContentHolder.SunTexture;
            moonTexture = ContentHolder.MoonTexture;

            circleOrigin = new Vector2(GlobalHelper.GameWidth / 2, GlobalHelper.GameHeight);
        }

        public void Update(GameTime gameTime)
        {
            // Update moon and sun position
            moonPos = circleOrigin + new Vector2(0, -RADIUS).RotateVector(inGameTimer.MoonRotation);
            sunPos = circleOrigin + new Vector2(0, -RADIUS).RotateVector(inGameTimer.MoonRotation + (float)Math.PI);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //// draw day sky texture
            //spriteBatch.Draw(dayTexture, Vector2.Zero, 
            //    Color.FromNonPremultiplied(255, 255, 255, (int)(inGameTimer.DaylightFraction * 255)));
            
            //// draw night sky texture
            //spriteBatch.Draw(nightTexture, Vector2.Zero,
            //    Color.FromNonPremultiplied(255, 255, 255, (int)((1-inGameTimer.DaylightFraction) * 255)));

            // draw sun and moon
            spriteBatch.Draw(sunTexture, sunPos, null, Color.White, 0, GetSunTexOrigin(), 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(moonTexture, moonPos, null, Color.White, 0, GetMoonTexOrigin(), 1f, SpriteEffects.None, 1f);
        }

        Vector2 GetSunTexOrigin()
        {
            return new Vector2(sunTexture.Width / 2, sunTexture.Height / 2);
        }

        Vector2 GetMoonTexOrigin()
        {
            return new Vector2(moonTexture.Width / 2, moonTexture.Height / 2);
        }
    }
}
