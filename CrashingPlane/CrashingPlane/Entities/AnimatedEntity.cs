using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrashingPlane.Entities
{
    abstract class AnimatedEntity : BasicEntity
    {
        Point sheetSize, frameSize, collisionSize;
        int msPerFrame;

        int timeSinceLastFrame = 0;
        Point currPoint = new Point(0, 0);

        /// <summary>
        /// This indicates whether this entity shall be removed
        /// </summary>
        public bool DeathAnimationPlayed
        {
            get { return (currPoint.Y == 1 && currPoint.X >= sheetSize.X - 1); }
        }

        public AnimatedEntity(Texture2D texture, Point sheetSize, Point frameSize, Point collisionSize, int msPerFrame)
            : base(texture)
        {
            this.sheetSize = sheetSize;
            this.frameSize = frameSize;
            this.collisionSize = collisionSize;
            this.msPerFrame = msPerFrame;
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > msPerFrame)
            {
                timeSinceLastFrame -= msPerFrame;
                currPoint.X++;
                if (currPoint.X >= sheetSize.X)
                {
                    if (isAlive)
                        currPoint.X = 0;
                    else
                        currPoint.X--;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public override void SetDead()
        {
            currPoint = new Point(0, 1);
            base.SetDead();
        }
    }
}
