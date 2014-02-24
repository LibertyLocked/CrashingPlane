using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrashingPlane.Entities
{
    public class Chopper
    {
        // gameplay variables
        float fuel;
        public const float MAX_FUEL = 100;
        bool isAlive = true;

        // graphics & animation variables
        Point frameSize = new Point(270, 200);    // constant, size of a sprite in sheet
        Point currentPoint = new Point(0, 0);
        Point sheetSize = new Point(6, 2);   // constant, num of animations in a spritesheet
        Point collisionSize = new Point(270, 100);  // constant, actual size of the chopper
        int timeSinceLastFrame = 0, msPerFrame = 20;
        Texture2D chopperSheet;

        // physics variables and constants
        float x = 200, y = 400, velocityY, accelerationY;
        float force;    // player can only control this
        const float MASS = 1f;
        const float MAX_V = 200f;
        const float G_CONST = 70f;
        const float FORCE_TO_APPLY = 160f;

        /// <summary>
        /// Gets the bounding rectangle of the plane for collision tests.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get { return new Rectangle((int)(x - collisionSize.X / 2.0), (int)(y - collisionSize.Y / 2.0), 
                collisionSize.X, collisionSize.Y); }
        }

        public float Fuel
        {
            get { return fuel; }
        }

        /// <summary>
        /// This indicates whether the backgrounds should still be scrolling.
        /// </summary>
        public bool IsAlive
        {
            get { return isAlive; }
        }

        /// <summary>
        /// This indicates whether game over screen shall be displayed.
        /// </summary>
        public bool DeathAnimationPlayed
        {
            get { return (currentPoint.Y == 1 && currentPoint.X >= sheetSize.X - 1); }
        }

        /// <summary>
        /// Returns Y coord of the chopper.
        /// </summary>
        public float Y
        {
            get { return y; }
        }

        public float VelocityY
        {
            get { return velocityY; }
        }

        public float Acceleration
        {
            get { return accelerationY; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Chopper()
        {
            chopperSheet = ContentHolder.ChopperSheet;
        }

        public void Update(GameTime gameTime)
        {
            // update physics only when player is alive
            if (isAlive)
            {
                // mg - F = ma, a = g - F/m
                accelerationY = G_CONST - force / MASS;

                // v = v0 + a * dt
                velocityY += accelerationY * (float)gameTime.ElapsedGameTime.TotalSeconds;
                velocityY = MathHelper.Clamp(velocityY, -MAX_V, MAX_V);

                // y = y0 + v * dt
                y += velocityY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // reset force after applied
                force = 0;
            }

            // Update animations
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > msPerFrame)
            {
                timeSinceLastFrame -= msPerFrame;
                currentPoint.X++;
                if (currentPoint.X >= sheetSize.X)
                {
                    if (isAlive)
                        currentPoint.X = 0; // because death animation is only played once
                    else
                        currentPoint.X--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw chopper sheet
            spriteBatch.Draw(chopperSheet, new Vector2(x - frameSize.X / 2, y - frameSize.Y / 2),
                new Rectangle(currentPoint.X * frameSize.X, currentPoint.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White);
        }

        /// <summary>
        /// Called when a deadly collision occurs. Sets player dead, and death animation is played.
        /// </summary>
        public void SetDead()
        {
            isAlive = false;
            currentPoint = new Point(0, 1);
        }

        /// <summary>
        /// Apply a force to the chopper. Has to be called every frame when holding.
        /// </summary>
        public void ApplyForce()
        {
            //velocityY = -500;
            force = FORCE_TO_APPLY;
        }

        public void AddFuel(float add)
        {
            fuel += add;
            fuel = MathHelper.Max(fuel, MAX_FUEL);
        }
    }
}
