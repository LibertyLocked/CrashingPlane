using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrashingPlane.Entities
{
    public class Bird : AnimatedEntity
    {
        float amplitude, period, wavelength; // we want the bird to fly in sine wave
        double totalSeconds;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public Bird()
        : base(ContentHolder.BirdSheet, new Point(10, 2), new Point(50, 30), new Point(45, 25), 30)
        {
            // TODO: Change values of sheetSize, frameSize and collisionSize according to the spritesheet.
            this.amplitude = 100;
            this.period = 1;
            this.wavelength = -200;
            this.position = new Vector2(2000, 500); // starting position of bird
            this.velocity = Vector2.Zero;   // bird flies in sine wave and V is calculated every frame.
        }
        
        public override void Update(GameTime gameTime)
        {
            totalSeconds += gameTime.ElapsedGameTime.TotalSeconds;
            velocity = GetVelocity(totalSeconds);
            base.Update(gameTime);
        }
        
        /// <summary>
        /// Gets the velocity based on time.
        /// </summary>
        private Vector2 GetVelocity(double totalSeconds)
        {
            float vX, vY;
            // vX is wavelength/period (wave velocity)
            vX = wavelength / period;
            // vY is obtained from cos wave of same amplitude and wavelength
            vY = amplitude * (float)Math.Cos(totalSeconds * vX / wavelength * MathHelper.TwoPi);
            
            return new Vector2(vX, vY);
        }
    }
}
