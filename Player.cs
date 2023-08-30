// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 
using System.Collections.Generic;
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
        private const int PlayerScale = 80;

        // Instance variables
        private int radius;
        private Vector2 position;
        private int speed;
        private int velocity;
        private bool start;
        private KeyboardState keyboardStateOld;
        private List<Texture2D> sprites;
        private Texture2D currentSprite;

        // Constructor
        public Player(GraphicsDeviceManager graphics, List<Texture2D> sprites) {
            radius = DefaultRadius;
            position = new Vector2((graphics.PreferredBackBufferWidth/2) - radius, graphics.PreferredBackBufferHeight/2 - radius);
            speed = DefaultSpeed;
            velocity = DefaultVelocity;
            start = false;  
            keyboardStateOld = Keyboard.GetState();
            this.sprites = sprites;
            currentSprite = sprites[0];
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
            
            // Change skins if applicable 
            setCurrentSprite();

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
                if(keyboardState.IsKeyDown(Keys.Space) && keyboardStateOld.IsKeyUp(Keys.Space) && position.Y > 0) {
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
            spriteBatch.Draw(currentSprite,playerPosition,Color.White);
        } 

        /**
         * SetDefaultPosition Method: 
         * 
         * resets player position back to the default position 
         *
         */
        public void SetDefaultPosition(GraphicsDeviceManager graphics){ 
            this.position.X = (graphics.PreferredBackBufferWidth/2) - radius; 
            this.position.Y = (graphics.PreferredBackBufferHeight/2) - radius;
        }
        
        /**
         * SetCurrentSprite Method:
         *
         * changes the current sprite based on
         * keyboard input (Arrow keys)
         *
         * @param: N/A
         * @return: N/A
         */
        public void SetCurrentSprite() {
            
            // Variables
            int size = sprites.Count;
            int currentIndex = sprites.IndexOf(currentSprite);
            KeyboardState keyboardState = Keyboard.GetState(); 
            
            // Change skins
            if (start) {
                if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyUp(Keys.Up)) {
                    if (currentIndex >= size - 1) {
                        currentSprite = sprites[0];
                        keyboardStateOld = keyboardState;
                        return;
                    }
                    else {
                        currentSprite = sprites[currentIndex + 1]; 
                        keyboardStateOld = keyboardState;
                        return;
                    }
                } 
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyUp(Keys.Down)) {
                    if (currentIndex <= 0) {
                        currentSprite = sprites[size - 1];
                        keyboardStateOld = keyboardState;
                        return;
                    }
                    else {
                        currentSprite = sprites[currentIndex - 1]; 
                        keyboardStateOld = keyboardState;
                        return;
                    }
                        
                }
            }
        }
          
        
    } 

}
