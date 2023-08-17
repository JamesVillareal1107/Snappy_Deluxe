using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snappy_Deluxe {
    internal class Pipe {

        // Constants 
        private const int defaultX = 1000;
        private const int defaultY = 760;
        private const int width = 130;
        private const int height = 800;
        private const int halfWidth= 65;
        private const int halfHeight = 400;
        private const int defaultSpeed = 300;

        // Instance Variables
        private Texture2D sprite;
        private Vector2 position;
        private int speed;

        // Constructors 
        public Pipe(Texture2D sprite) {
            this.sprite = sprite;
            this.speed = defaultSpeed;
            this.position = new Vector2(defaultX, defaultY);
        }

        // Class Properties 
        public Texture2D Sprite{
            get { return sprite;}
            set { sprite = value; }
        }

        public Vector2 Position { 
            get { return position;} 
            set { position = value; }
        }

        public int Speed {
            get { return speed; }
            set { speed = value; }
        }

        // Class Methods

        /**
         * Update method: 
         * 
         * Move pipe left every frame
         * 
         */
        public void Update(GameTime gameTime) {            
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            position.X -= speed * deltaTime;
        }

        /**
         * Draw method:
         * 
         * Draw pipe sprite at appropriate location
         * based on position and scale variables
         * 
         */
        public void Draw(SpriteBatch spriteBatch) { 
            Rectangle drawPosition = new Rectangle((int)position.X-halfWidth, (int)position.Y-halfHeight, width, height);
            spriteBatch.Draw(sprite, drawPosition, Color.White);
        }

    }
}
