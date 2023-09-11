// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic; 
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Snappy_Deluxe {
    internal class GameManager {

        // Constants 
        private const int DefaultWidth = 1280;
        private const int DefaultHeight = 720;
        private const double DefaultTime = 1.3;
        private const int DefaultScore = 0;
        private const int MaxSpawnOffset = 115;
        private const int ScoreYPosition = 5;
        private const int FontRadius = 45;
        private const int TitleRadius = 293;
        private const int PipeDeletionPoint = -300;
        private const int HighScoreRadius = 288;
        private const int HighScoreYPositionOffset = 150;
        private const double ScoreValue = 0.5;
        private const int CollisionOffset = 1;
        private const int GroundCollision = 96;
        private const float DefaultDeathVolume = 0.5f;
        private const float DefaultDeathPitch = 1f;
        private const float DefaultDeathPan = 0f;
        private const int GroundHeight = 96;

        // Instance Variables
        private bool inGameLoop;
        private bool startOfGame;
        private double spawnTimer;
        private double score;
        private double highScore;
        private bool collided;


        // Constructors
        public GameManager() {
            inGameLoop = false;
            startOfGame = false;
            spawnTimer = DefaultTime;
            score = DefaultScore;
            highScore = DefaultScore;
            collided = false;
        }

        // Properties 
        public bool InGameLoop {
            get { return inGameLoop; }
            set { inGameLoop = value; }
        } 
        
        public bool Collided {
            get { return collided; }
            set { collided = value; }
        }

        public bool StartOfGame {
            get { return startOfGame; }
            set { startOfGame = value; }
        }

        public double SpawnTimer {
            get { return spawnTimer; }
            set { spawnTimer = value; }
        }

        public double Score {
            get { return score; }
            set { score = value; }
        }

        public double HighScore {
            get { return highScore; }
            set { highScore = value; }
        }
    
        /** 
         * Update method: 
         *
         * Implements game logic based on the state
         * of the game 
         *
         */
        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, Player player, Random spawnOffset,
                List<Pipe> pipesList, Texture2D topPipeSprite, Texture2D bottomPipeSprite)
        {

            // if the game is not started, make sure collided is false 
            if (!inGameLoop)
            {
                collided = false;
            }

            // Update player and cast score to int at all times 
            player.Update(gameTime, this);
            score = (int)score;

            // Game Start logic 
            StartGame(player);

            // Main game loop
            if (inGameLoop)
            {

                // Start/setup main game loop 
                if (startOfGame)
                {
                    startOfGame = false;
                    PipeSpawner.SpawnPipes(topPipeSprite, bottomPipeSprite, pipesList);
                }


                // Spawn Pipes 
                if (!collided)
                {
                    spawnTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (spawnTimer <= 0)
                    {
                        int spawnPoint = spawnOffset.Next(-MaxSpawnOffset, MaxSpawnOffset);
                        PipeSpawner.SpawnPipesRandom(topPipeSprite, bottomPipeSprite, pipesList, spawnPoint);
                        spawnTimer = DefaultTime;
                    }
                }

                // Update Pipes and increment score when applicable
                foreach (Pipe pipe in pipesList)
                {
                    if (CollisionDetected(graphics, player, pipe) && !collided)
                    {
                        collided = true;
                        Sounds.birdDying.Play();
                    }
                    if (!collided)
                    {
                        pipe.Update(gameTime);
                        ScoreCheck(player, pipe);
                        DeletePipe(pipe, pipesList);
                    }
                    if (collided)
                    {
                        player.Start = false;
                    }
                    if (IsPlayerOutOfBounds(player, graphics))
                    {
                        inGameLoop = false;
                        spawnTimer = DefaultTime;
                        score = DefaultScore;
                        player.SetDefaultPosition(graphics);
                        foreach (Pipe remainingPipe in pipesList)
                        {
                            remainingPipe.Deleted = true;
                        }
                        player.Start = false;
                        Sounds.deathSound.Play(DefaultDeathVolume, DefaultDeathPitch, DefaultDeathPan);
                        collided = false;
                    }
                }

            }

            // Delete all pipes
            pipesList.RemoveAll(p => p.Deleted);
        }

        /* 
         * StarGame Method: 
         * start the game based on player input 
         * 
         * @param: player <Player> 
         * @return: N/A  
         *  
         */ 
        private void StartGame(Player player){
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space) && !inGameLoop && player.Start){
                inGameLoop = true;
                startOfGame = true;
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
                List<Pipe> pipesList, SpriteFont spriteFont, SpriteFont messageSpriteFont)
        {
            // Drawing variables 
            Rectangle backgroundPosition;
            Vector2 scorePosition, titlePosition, highScorePosition, skinMessagePosition, startMessagePosition;
            backgroundPosition = new Rectangle(0, 0, DefaultWidth, DefaultHeight);
            scorePosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - FontRadius, ScoreYPosition);
            titlePosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - TitleRadius, ScoreYPosition);
            highScorePosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - HighScoreRadius, (graphics.PreferredBackBufferHeight / 2) + HighScoreYPositionOffset);
            skinMessagePosition = new Vector2(player.Position.X - 500, player.Position.Y);
            startMessagePosition = new Vector2(player.Position.X + 300, player.Position.Y);

            // Always draw background
            spriteBatch.Draw(backgroundSprite, backgroundPosition, null, Color.White);
            
            // Draw Based on game condition 
            if (inGameLoop) {
                foreach (Pipe pipe in pipesList) {
                    pipe.Draw(spriteBatch);
                }
                spriteBatch.DrawString(spriteFont, score.ToString(), scorePosition, Color.White);
            }
            else {
                spriteBatch.DrawString(spriteFont, "Snappy Deluxe", titlePosition, Color.White);
                spriteBatch.DrawString(spriteFont, "High Score: " + highScore.ToString(), highScorePosition, Color.White);
                spriteBatch.DrawString(messageSpriteFont, "Use up and down \nkeys to change skins", skinMessagePosition, Color.White);
                spriteBatch.DrawString(messageSpriteFont, "Press space to start", startMessagePosition, Color.White);
            } 
            player.Draw(spriteBatch); 
        }


        /**
         * Score Method: 
         *
         * increments score variables
         * when a player passes a group of pipes 
         *
         */
        public bool ScoreCheck(Player player, Pipe pipe) {
            if ((int)player.Position.X == (int)pipe.Position.X) {
                score += ScoreValue;
                Sounds.scoreSound.Play();
                if (score > highScore) {
                    highScore = (int)score;
                }
                return true;
            }
            return false;
        }

        /**
         * DeletePipe method: 
         *
         * Given a pipe, if it meets a certain x position 
         * the pipe object is removed from the game 
         *
         */
        public void DeletePipe(Pipe pipe, List<Pipe> pipesList) {
            if (pipe.Position.X <= PipeDeletionPoint) {
                pipe.Deleted = true;
            }
        }

        /** 
         * CollisionDetected:
         * 
         * Detects a collision between the player object and the 
         * pipe object  
         * 
         * @Param: graphics <GraphicsDeviceManager>
         * @Param: player <Player> 
         * @Param: pipe <Pipe>
         *
         */
        public bool CollisionDetected(GraphicsDeviceManager graphics, Player player, Pipe pipe) {
            
            // Don't detect collisions if collided is true 
            if (collided) {
                return false;
            }
            // Return true if player is out of bounds on the Y position 
            if (player.Position.Y >= graphics.PreferredBackBufferHeight - GroundCollision) {
                return true;
            }

            // check if the player is in the same x area as the pipe
            bool inXArea = false;
            int pipeLeftMaximum = (int)pipe.Position.X - pipe.HalfWidth;
            int pipeRightMaximum = (int)pipe.Position.X + pipe.HalfWidth;
            int playerLeftCheck = (int)(player.Position.X + player.Radius);
            int playerRightCheck = (int)(player.Position.X - player.Radius);
            if (playerLeftCheck > pipeLeftMaximum && playerRightCheck < pipeRightMaximum) {
                inXArea = true; 
            }

            // Determine if pipe is above or below the player 
            bool abovePlayer = false;
            if (pipe.Position.Y < player.Position.Y) {
                abovePlayer = true;
            }

            // Determine if a collision took place 
            if (inXArea && abovePlayer) {
                int yCollisionVal = (int)(pipe.Position.Y + pipe.HalfHeight);
                if (player.Position.Y < yCollisionVal) {
                    return true;
                }
            }
            else if (inXArea && !abovePlayer) {
                int yCollisionVal = (int)(pipe.Position.Y - pipe.HalfHeight);
                if (player.Position.Y > yCollisionVal) {
                    return true;
                }
            }

            // Default return value (no collision took place)            
            return false;
        }
        
        /**
         * IsPLayerOutOfBounds method:
         *
         * returns whether or not the player
         * is out of bounds
         *
         * @Param: player <Player>
         * @Param: graphics <GraphicsDeviceManager>
         * @Return: boolean representing if the player is out of bounds
         */
        public bool IsPlayerOutOfBounds(Player player, GraphicsDeviceManager graphics) { 
            
            // variables representing screen boundaries & player position
            int left = 0; 
            int top = 0;
            int bottom = graphics.PreferredBackBufferHeight - GroundHeight;
            int playerPositionX = (int)player.Position.X;
            int playerPositionY = (int)player.Position.Y; 
            
            // logic to detect if player is out of bounds 
            if (playerPositionX <= left) {
                return true;
            }
            if (playerPositionY <= top || playerPositionY >= bottom) {
                return true;
            }

            // Default return
            return false;
        }
    } 

}
