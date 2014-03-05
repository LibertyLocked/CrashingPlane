using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CrashingPlane.Entities
{
    public abstract class BasicEntity
    {
        protected Texture2D entityTexture;
        protected Point collisionSize;
        protected Vector2 position, velocity;
        protected bool isAlive = true;

        /// <summary>
        /// The bounding rectangle of this entity.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - collisionSize.X / 2.0), (int)(position.Y - collisionSize.Y / 2.0),
                collisionSize.X, collisionSize.Y);
            }
        }

        /// <summary>
        /// This indicates whether this entity shall still be used in collision tests.
        /// </summary>
        public bool IsAlive
        {
            get { return isAlive; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BasicEntity(Texture2D texture, Point collisionSize)
        {
            this.entityTexture = texture;
            this.collisionSize = collisionSize;
        }

        public virtual void Update(GameTime gameTime)
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(entityTexture, position, null, Color.White, 0,
                new Vector2(entityTexture.Width / 2, entityTexture.Height / 2),
                1f, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Called by world when a collision occurs.
        /// </summary>
        public virtual void Trigger(Chopper player)
        {
            //if (!player.IsAlive)
            //    this.SetDead();
        }

        public virtual void SetDead()
        {
            isAlive = false;
        }
        
        protected void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
    }
}
