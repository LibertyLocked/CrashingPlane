using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CrashingPlane.Mapping;
using CrashingPlane.Entities;

namespace CrashingPlane
{
    public class World
    {
        Texture2D blank;
        List<ScrollingBackground> backgrounds = new List<ScrollingBackground>();
        public InGameTimer inGameTimer;
        SkyBackground skyBackground;

        public Chopper Player;
        List<FuelCan> fuelCans = new List<FuelCan>();
        List<Bird> birds = new List<Bird>();

        int timeWaited = 0;
        int timeToWait = 0;
        Random rnd = new Random();

        /// <summary>
        /// Returns true if player is dead and explosion is played.
        /// </summary>
        public bool EndOfGameplay
        {
            get { return !Player.IsAlive && Player.DeathAnimationPlayed; }
        }

        /// <summary>
        /// Gets the time spent since last time in-game timer resets
        /// </summary>
        public TimeSpan TimeSpentFlying
        {
            get { return inGameTimer.InGameTimeSpan; }
        }

        public DateTime InGameTime
        {
            get { return inGameTimer.InGameTime; }
        }

        public World()
        {
            blank = ContentHolder.Blank;
            inGameTimer = new InGameTimer();
            skyBackground = new SkyBackground(inGameTimer);

            // add all backgrounds
            backgrounds.Add(new ScrollingBackground(ContentHolder.BackgroundClouds, -100, true));
            backgrounds.Add(new ScrollingBackground(ContentHolder.BackgroundMountain1, -50, false));
            backgrounds.Add(new ScrollingBackground(ContentHolder.BackgroundGround, -600, false));
            backgrounds.Add(new ScrollingBackground(ContentHolder.BackgroundTree1, -600, false));
            backgrounds.Add(new ScrollingBackground(ContentHolder.BackgroundTree2, -1200, false));
        }

        public void Update(GameTime gameTime)
        {
            // wait timers
            if (timeToWait != 0)
            {
                timeWaited += gameTime.ElapsedGameTime.Milliseconds;
                if (timeWaited > timeToWait)
                {
                    timeToWait = 0;
                    timeWaited = 0;
                }
                return;
            }

            // Update backgrounds and timer when player is not defined or is alive
            if (Player == null || Player.IsAlive)
            {
                // update sky background
                skyBackground.Update(gameTime);

                // update scrolling backgrounds
                foreach (ScrollingBackground bg in backgrounds)
                    bg.Update(gameTime);

                // update in-game timer
                inGameTimer.Update(gameTime);
            }

            // update player chopper
            if (Player != null) // update even when player is dead because it needs to play animation
                Player.Update(gameTime);

            // Collision detections when player is alive
            if (Player != null && Player.IsAlive)
                CollisionDetections();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw sky
            // first fill render target with a solid background
            spriteBatch.Draw(blank, new Rectangle(0, 0, GlobalHelper.GameWidth, GlobalHelper.GameHeight),
                Color.FromNonPremultiplied((int)(135 * inGameTimer.Brightness),
                (int)(206 * inGameTimer.Brightness),
                (int)(250 * inGameTimer.Brightness), 
                255));
            // then draw the sky background
            skyBackground.Draw(spriteBatch);

            // draw scrolling backgrounds
            foreach (ScrollingBackground bg in backgrounds)
            {
                bg.Draw(spriteBatch);
            }

            // draw player chopper
            if (Player != null)
                Player.Draw(spriteBatch);

            // apply brightness last (using a black rectangle)
            spriteBatch.Draw(blank, new Rectangle(0, 0, GlobalHelper.GameWidth, GlobalHelper.GameHeight), 
                Color.FromNonPremultiplied(0, 0, 0, (int)(255 * (1 - inGameTimer.Brightness))));
        }

        /// <summary>
        /// Should be called only by Gameplay Screen, when game resets or just initializes.
        /// </summary>
        public void ResetEntities()
        {
            Player = new Chopper();
            inGameTimer.Reset();
        }

        /// <summary>
        /// Called by Gameplay Screen, applies an upward force to player chopper. Only works when player is alive.
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForceToPlayer()
        {
            if (Player.IsAlive)
                Player.ApplyForce();
        }

        private void CollisionDetections()
        {
            // player vs game border
            if (Player.Y < 100 || Player.Y > GlobalHelper.GameHeight - 100)
                Player.SetDead();

            // player vs powerups

            // player vs birds
        }

        /// <summary>
        /// Pauses the update of world for a certain amount of time.
        /// </summary>
        /// <param name="countdownTime"></param>
        public void Wait(int countdownTime)
        {
            timeWaited = 0;
            timeToWait = countdownTime;
        }
    }
}
