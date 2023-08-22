// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Snappy_Deluxe {
    internal class Player {

        // Constants 
        private const int DefaultRadius = 40;
        private const int DefaultSpeed = 400;
        private const int DefaultVelocity = 0; 
        private const int MaxVelocity = 1000;
        private const int VelocityChange = 35;
        private const int xOffset = 100;
        private const int PlayerScale = 80;

        // Instance variables
        private int radius;
        private Vector2 position;
        private int speed;
        private int velocity;
        private bool start;
        private KeyboardState keyboardStateOld;
        private Texture2D playerSprite;

        // Constructor
        public Player(GraphicsDeviceManager graphics, Texture2D playerSprite) {
            radius = DefaultRadius;
            position = new Vector2((graphics.PreferredBackBufferWidth/2-xOffset) - radius, graphics.PreferredBackBufferHeight/2 - radius);
            speed = DefaultSpeed;
            velocity = DefaultVelocity;
            start = false;  
            keyboardStateOld = Keyboard.GetState();
            this.playerSprite = playerSprite; 
        }

        // Properties/Getters
        public int Radius{
            get { return radius; } 
        } 
        
        public Vector2 Position {
            get { return position; } 
            set { position = value; }
        }
        

        /** 
         * Update method: 
         * 
         * Runs every frame  
         * simulates gravity and player jumping 
         * Starts game when prompted
         * 
         * @param gameTime 
         */
        public void Update(GameTime gameTime) {

            // Game start
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space)) {
                start = true;
            }

            // Vertical movement
            if (start) { 
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; 
                float verticalTrajectory = (speed * deltaTime) - (velocity * deltaTime);
                position.Y += verticalTrajectory; 
                if(keyboardState.IsKeyDown(Keys.Space) && keyboardStateOld.IsKeyUp(Keys.Space)) {
                    velocity = MaxVelocity; 
                }
                velocity -= VelocityChange;
            }  
            keyboardStateOld = keyboardState; 
        }  
        
        /** 
         * Draw Method: 
         *
         * Draws Player Character when 
         * called 
         *
         */
        public void Draw(SpriteBatch spriteBatch){ 
            Rectangle playerPosition = new Rectangle((int)position.X,(int)position.Y,PlayerScale,PlayerScale);
            spriteBatch.Draw(playerSprite,playerPosition,Color.White);
        }

        /** 
         * SetDefaultPosition Method: 
         *
         * reverts player position back 
         * to default position 
         *
         */
        public void SetDefaultPosition(GraphicsDeviceManager graphics){ 
            this.position.X = (graphics.PreferredBackBufferWidth/2-xOffset) - radius;   
            this.position.Y = (graphics.PreferredBackBufferHeight/2) - radius;
        }
        
    } 

}
