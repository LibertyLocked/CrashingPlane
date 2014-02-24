using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrashingPlane.Mapping
{
    public class InGameTimer
    {
        TimeSpan inGameTimeSpan;
        DateTime initialTime;

        float brightness;
        float sunAlpha;
        float moonRotation;

        const float MIN_BRIGHTNESS = 0.3f;
        const float MAX_BRIGHTNESS = 1f;

        /// <summary>
        /// Gets the in-game time span.
        /// This is typically the time span player spent playing the game.
        /// </summary>
        public TimeSpan InGameTimeSpan
        {
            get { return inGameTimeSpan; }
        }

        /// <summary>
        /// Gets the in-game time. (initial datetime + in game timespan)
        /// </summary>
        public DateTime InGameTime
        {
            get { return initialTime.Add(inGameTimeSpan); }
        }

        /// <summary>
        /// Returns the fraction of gamescreen light. (1 = brightest, 0 = cantsee)
        /// </summary>
        public float Brightness
        {
            get { return brightness; }
        }

        /// <summary>
        /// Returns the alpha fraction for daylight. (1 = full sun, 0 = full moon)
        /// </summary>
        public float DaylightFraction
        {
            get { return sunAlpha; }
        }

        /// <summary>
        /// Returns the rotation of moon. (0 = midnight, pi = noon)
        /// </summary>
        public float MoonRotation
        {
            get { return moonRotation; }
        }

        public InGameTimer()
        {
            initialTime = DateTime.Now;
            inGameTimeSpan = TimeSpan.Zero;
        }

        public void Update(GameTime gameTime)
        {
            // let 1 sec irl = 5 mins (300 sec) in game, therefore 1 ms irl = 300 ms in game
            inGameTimeSpan = inGameTimeSpan.Add(new TimeSpan(0, 0, 0, 0, gameTime.ElapsedGameTime.Milliseconds * 300));

            // update brightness fraction
            UpdateBrightness(InGameTime.TimeOfDay);

            // update sun alpha
            UpdateSunAlpha(InGameTime.TimeOfDay);

            // update moon rotation
            UpdateMoonRotation(InGameTime.TimeOfDay);
        }

        /// <summary>
        /// Resets the in-game timespan to zero, and sets initial datetime to current in-game time.
        /// </summary>
        public void Reset()
        {
            initialTime = InGameTime;
            inGameTimeSpan = TimeSpan.Zero;
        }

        private void UpdateBrightness(TimeSpan timeOfDay)
        {
            // 12 o'clock (0.5) is the brightest, 0/24 o'clock (0/1) is darkest
            float rawBrightness = 1 - Math.Abs((float)timeOfDay.TotalDays - 0.5f) * 2;
            brightness = MIN_BRIGHTNESS + rawBrightness * (MAX_BRIGHTNESS - MIN_BRIGHTNESS);
        }

        private void UpdateSunAlpha(TimeSpan timeOfDay)
        {
            sunAlpha = 1 - Math.Abs((float)timeOfDay.TotalDays - 0.5f) * 2; ;
        }

        private void UpdateMoonRotation(TimeSpan timeOfDay)
        {
            moonRotation = (float)(timeOfDay.TotalDays * MathHelper.TwoPi);
        }
    }
}
