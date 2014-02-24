using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CrashingPlane.Entities
{
    abstract class BasicEntity
    {
        Texture2D entityTexture;
        Vector2 position, velocity;
        protected bool isAlive = true;

        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - entityTexture.Width / 2.0), (int)(position.Y - entityTexture.Height / 2.0),
                entityTexture.Width, entityTexture.Height);
            }
        }

        /// <summary>
        /// This indicates whether this entity shall still be used in collision tests.
        /// </summary>
        public bool IsAlive
        {
            get { return isAlive; }
        }

        public BasicEntity(Texture2D texture)
        {
            this.entityTexture = texture;
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

        public virtual void Trigger(Chopper player)
        {
            if (!player.IsAlive)
                this.SetDead();
        }

        public virtual void SetDead()
        {
            isAlive = false;
        }
    }
}
