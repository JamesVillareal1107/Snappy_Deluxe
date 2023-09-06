// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Snappy_Deluxe {
    internal class Player {

        // Constants 
        private const int DefaultRadius = 40;
        private const int DefaultGravity = 400;
        private const int DefaultVelocity = 0; 
        private const int MaxVelocity = 1000;
        private const int VelocityChange = 35;
        private const int PlayerScale = 80;
        private const float UpwardsRotation = 0.013f;
        private const float DownwardsRotation = 0.02f;
        private const float RotationOffset = 0.5f;

        // Instance variables
        private int radius;
        private Vector2 position;
        private int gravity;
        private int velocity;
        private bool start;
        private KeyboardState keyboardStateOld;
        private List<Texture2D> sprites;
        private Texture2D currentSprite;
        private float rotation;

        // Constructor
        public Player(GraphicsDeviceManager graphics, List<Texture2D> sprites) {
            radius = DefaultRadius;
            position = new Vector2((graphics.PreferredBackBufferWidth/2) - radius, graphics.PreferredBackBufferHeight/2 - radius);
            gravity = DefaultGravity;
            velocity = DefaultVelocity;
            start = false;  
            keyboardStateOld = Keyboard.GetState();
            this.sprites = sprites;
            currentSprite = sprites[0];
            rotation = 0;
        }

        // Properties/Getters
        public int Radius{
            get { return radius; } 
        } 
        
        public Vector2 Position {
            get { return position; } 
            set { position = value; }
        }

        public bool Start {
            get { return start; }
            set { start = value; }
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
            SetCurrentSprite();

            // Game start
            var startState = SetStartMovement();

            // movement logic
            VerticalMovement(gameTime, startState); 
            Rotate();
        }
        
        /**
         * StartMovement method:
         *
         * Starts initial player movement
         * when called
         *
         * @param: N/A
         * @return: Keyboardstate
         */
        private KeyboardState SetStartMovement() {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space) && keyboardStateOld.IsKeyUp(Keys.Space)) {
                start = true;
            }
            return keyboardState;
        }

        /**
         * Jump Method:
         *
         * Makes player character jump
         * when called
         *
         * @param: gameTime <GameTime>
         * @param: keyboardState <KeyboardState>
         */
        private void VerticalMovement(GameTime gameTime, KeyboardState keyboardState) {
            if (start) {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float verticalTrajectory = gravity - velocity;
                position.Y += verticalTrajectory * deltaTime;
                if (keyboardState.IsKeyDown(Keys.Space) && keyboardStateOld.IsKeyUp(Keys.Space) && position.Y > 0) {
                    velocity = MaxVelocity;
                    Sounds.jumpSound.Play();
                }
                velocity -= VelocityChange;
            }
            keyboardStateOld = keyboardState;
        }

        /** 
         * Draw Method: 
         *
         * Draws Player Character when 
         * called.
         *
         */
        public void Draw(SpriteBatch spriteBatch){ 
            Rectangle playerPosition = new Rectangle((int)position.X,(int)position.Y,PlayerScale,PlayerScale);
            spriteBatch.Draw(currentSprite,playerPosition, null , Color.White, MathHelper.ToRadians(rotation), new Vector2(RotationOffset,RotationOffset), SpriteEffects.None, 0); // TODO: fix
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
            if (!start) {
                if (keyboardState.IsKeyDown(Keys.Up) && keyboardStateOld.IsKeyUp(Keys.Up)) {
                    if (currentIndex >= size - 1) {
                        currentSprite = sprites.ElementAt(0);
                        keyboardStateOld = keyboardState;
                        return;
                    }
                    else {
                        currentSprite = sprites.ElementAt(currentIndex + 1); 
                        keyboardStateOld = keyboardState;
                        return;
                    }
                } 
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardStateOld.IsKeyUp(Keys.Down)) {
                    if (currentIndex <= 0) {
                        currentSprite = sprites.ElementAt(size - 1);
                        keyboardStateOld = keyboardState;
                        return;
                    }
                    else {
                        currentSprite = sprites.ElementAt(currentIndex - 1); 
                        keyboardStateOld = keyboardState;
                        return;
                    }
                        
                }
            }
        }
        
        /**
         * Rotate method:
         *
         * Changes the rotation value based on
         * the velocity/trajectory of the player object
         * when in game
         *
         * @param: N/A
         * @return: N/A
         */
        public void Rotate() {
            float verticalTrajectory = gravity - velocity;
            if (verticalTrajectory < 0 && start) {
                rotation = verticalTrajectory * UpwardsRotation;
            } 
            else if (verticalTrajectory > 0 && start) {
                rotation = verticalTrajectory * DownwardsRotation;
            }
            else {
                rotation = 0;
            }
        }
          
        
    }  
    

}
