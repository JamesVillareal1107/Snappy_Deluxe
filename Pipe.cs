using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snappy_Deluxe {
    internal class  Pipe {

        // Constants 
        private const int defaultX = 1000;
        private const int defaultY = 760;
        private const int DefaultHalfWidth = 65;
        private const int DefaultHalfHeight = 400;
        private const int defaultSpeed = 300; 
        private const int DefaultWidth = 130; 
        private const int DefaultHeight = 800;
        private const float ScaleValueWidth = 9.8f;
        private const float ScaleValueHeight = 0.9f;

        // Instance Variables
        private Texture2D sprite;
        private Vector2 position;
        private int speed; 
        private bool deleted; 
        private int halfWidth; 
        private int halfHeight;
        private int width;
        private int height;

        // Constructors 
        public Pipe(Texture2D sprite, int xPosition, int yPosition) {
            this.sprite = sprite;
            this.speed = defaultSpeed;
            this.position = new Vector2(xPosition, yPosition);
            this.deleted = false;
            this.width = DefaultWidth;
            this.height = DefaultHeight;
            this.halfWidth = DefaultHalfWidth;
            this.halfHeight = DefaultHalfHeight;
        }
        

        // Class Properties 
        public Texture2D Sprite {
            get { return sprite; }
            set { sprite = value; }
        }

        public Vector2 Position { 
            get { return position; } 
            set { position = value; }
        }

        public int Speed {
            get { return speed; }
            set { speed = value; }
        } 

        public bool Deleted{ 
            get { return deleted; }
            set { deleted = value; }
        } 

        public int HalfWidth{ 
            get { return halfWidth; }
            set { halfWidth = value; }
        } 

        public int HalfHeight{ 
            get { return halfHeight; }
            set { halfHeight = value; }
        }

        // Class Methods

        /**
         * Update method: 
         * 
         * Move pipe left every frame
         * 
         */
        public void Update(GameTime gameTime, GraphicsDeviceManager graphics) {            
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            position.X -= speed * deltaTime;
            setScale(graphics);
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

        public void setScale(GraphicsDeviceManager graphics) {
            width = (int)(graphics.PreferredBackBufferWidth / ScaleValueWidth);
            height = (int)(graphics.PreferredBackBufferHeight / ScaleValueHeight);
            halfWidth = width / 2; 
            halfHeight = height / 2;
        }

    }
}
