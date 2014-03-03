using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrashingPlane.Entities
{
    class FuelCan : BasicEntity
    {
        float rotation; // we want the texture to rotate 
        
        public FuelCan(Vector2 position, float velocityX)
            : base(ContentHolder.FuelCanTexture, new Point(20, 20))
        {
            // TODO: Change collisionSize according to the spritesheet
            this.position = position;
            this.velocity = new Vector2(velocityX, 0);
        }

        public override void Trigger(Chopper player)
        {
            if (player.IsAlive)
                player.AddFuel(20);
            base.Trigger(player);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Override this because we want to draw with rotation
            
        }
    }
}
