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
        public FuelCan()
            : base(ContentHolder.FuelCanTexture)
        { }

        public override void Trigger(Chopper player)
        {
            if (player.IsAlive)
                player.AddFuel(20);
            base.Trigger(player);
        }
    }
}
