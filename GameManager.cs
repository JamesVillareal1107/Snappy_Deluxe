// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic; 
using System;

namespace Snappy_Deluxe{ 
	internal class GameManager { 
        
        // Constants 
        private const int DefaultWidth = 1280;
        private const int DefaultHeight = 720;
        private const double DefaultTime = 1.5;
        private const int DefaultScore = 0;
        private const int MaxSpawnOffset = 180; 
        private const int TopTextPosition = 5; 
        private const int FontRadius = 45;
        private const int TitleRadius = 293;

        // Instance Variables
        private bool inGameLoop;
        private bool startOfGame;
        private double timer; 
        private double score;

        // Constructors
        public GameManager(){ 
            inGameLoop = false;
            startOfGame = false;
            timer = DefaultTime;
            score = DefaultScore;
        }

        // Properties 
        public bool InGameLoop{ 
            get { return inGameLoop; }
            set { inGameLoop = value; }
        } 

        public bool StartOfGame{
            get { return startOfGame; }
            set { startOfGame = value; }
        }

        public double Timer{ 
            get { return timer; } 
            set { timer = value; }
        }

        public double Score{ 
            get { return score; } 
            set { score = value; }
        }

        // Methods

        /** 
         * Update method: 
         *
         * Implements game logic based on the state
         * of the game 
         *
         */
        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, Player player, Random spawnOffset,
                List<Pipe> pipesList, Texture2D topPipeSprite, Texture2D bottomPipeSprite){ 
            
            // Game Start logic 
            KeyboardState keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.Space) && !inGameLoop){
                inGameLoop = true;
                startOfGame = true;
            }

            //TODO: Condition if we are in the main game loop 
            if(inGameLoop){
               
                // Start/setup main game loop 
                if(startOfGame){ 
                    startOfGame = false;
                    PipeSpawner.SpawnPipes(topPipeSprite, bottomPipeSprite, pipesList);
                }

                // Update player
                player.Update(gameTime);

                // Spawn Pipes
                timer -= gameTime.ElapsedGameTime.TotalSeconds;
                if(timer <= 0){
                    int spawnPoint = spawnOffset.Next(-MaxSpawnOffset,MaxSpawnOffset); 
                    PipeSpawner.SpawnPipesRandom(topPipeSprite, bottomPipeSprite, pipesList, spawnPoint);
                    timer = DefaultTime;    
                }

                // Update Pipes and increment score when applicable
                foreach(Pipe pipe in pipesList){
                    pipe.Update(gameTime);
                    ScoreCheck(player,pipe);  
                }

            }

        }
       
        /**
         * Draw Method: 
         *
         * Draws appropriate game objects
         * based on the state of 
         * the game 
         *
         */
       public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D backgroundSprite, Player player, Random spawnOffset,
               List<Pipe> pipesList, SpriteFont spriteFont){ 
            
            // Drawing variables 
            Rectangle backgroundPosition = new Rectangle(0,0,DefaultWidth,DefaultHeight); 
            Vector2 textPosition = new Vector2((graphics.PreferredBackBufferWidth/2) - FontRadius, TopTextPosition);
            Vector2 titlePosition = new Vector2((graphics.PreferredBackBufferWidth/2) - TitleRadius, TopTextPosition);

            // Always draw background and player
            spriteBatch.Draw(backgroundSprite, backgroundPosition, Color.White);  
            player.Draw(spriteBatch);

            // Draw Based on game condition 
            if(inGameLoop){
                foreach(Pipe pipe in pipesList){ 
                    pipe.Draw(spriteBatch);
                } 
                spriteBatch.DrawString(spriteFont, score.ToString(), textPosition, Color.White);
            }
            else {
                spriteBatch.DrawString(spriteFont, "Snappy Deluxe", titlePosition, Color.White);
            }
       }

       /**
        * Score Method: 
        *
        * increments score variables
        * when a player passes a group of pipes 
        *
        */
       public bool ScoreCheck(Player player, Pipe pipe){ 
            if(player.Position.X == pipe.Position.X){
                score += 0.5; 
                return true;
            }  
            return false;
       }

	}	
}
