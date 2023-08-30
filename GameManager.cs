// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic; 
using System;
using Microsoft.Xna.Framework.Media;
using System.Runtime.InteropServices;

namespace Snappy_Deluxe {
    internal class GameManager {

        // Constants 
        private const int DefaultWidth = 1280;
        private const int DefaultHeight = 720;
        private const double DefaultTime = 1.5;
        private const int DefaultScore = 0;
        private const int MaxSpawnOffset = 150;
        private const int ScoreYPosition = 5;
        private const int FontRadius = 45;
        private const int TitleRadius = 293;
        private const int PipeDeletionPoint = -300;
        private const int HighScoreRadius = 288;
        private const int HighScoreYPositionOffset = 150;
        private const double ScoreValue = 0.5;
        private const int CollisionOffset = 20;
        private const int GroundCollision = 96;

        // Instance Variables
        private bool inGameLoop;
        private bool startOfGame;
        private double spawnTimer;
        private double score;
        private double highScore;


        // Constructors
        public GameManager() {
            inGameLoop = false;
            startOfGame = false;
            spawnTimer = DefaultTime;
            score = DefaultScore;
            highScore = DefaultScore;
        }

        // Properties 
        public bool InGameLoop {
            get { return inGameLoop; }
            set { inGameLoop = value; }
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
                List<Pipe> pipesList, Texture2D topPipeSprite, Texture2D bottomPipeSprite) { 
            
            // Update player
            player.Update(gameTime);


            // Game Start logic 
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space) && !inGameLoop) {
                inGameLoop = true;
                startOfGame = true;
            }

            //TODO: Condition if we are in the main game loop 
            if (inGameLoop) {

                // Start/setup main game loop 
                if (startOfGame) {
                    startOfGame = false;
                    PipeSpawner.SpawnPipes(topPipeSprite, bottomPipeSprite, pipesList);
                }
                

                // Spawn Pipes
                spawnTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (spawnTimer <= 0) {
                    int spawnPoint = spawnOffset.Next(-MaxSpawnOffset, MaxSpawnOffset);
                    PipeSpawner.SpawnPipesRandom(topPipeSprite, bottomPipeSprite, pipesList, spawnPoint);
                    spawnTimer = DefaultTime;
                }

                // Update Pipes and increment score when applicable
                foreach (Pipe pipe in pipesList) {
                    pipe.Update(gameTime);
                    ScoreCheck(player, pipe);
                    DeletePipe(pipe, pipesList);
                    if (CollisionDetected(graphics, player, pipe)) {
                        inGameLoop = false;
                        spawnTimer = DefaultTime;
                        score = DefaultScore;
                        player.SetDefaultPosition(graphics);
                        foreach (Pipe remainingPipe in pipesList) {
                            remainingPipe.Deleted = true;
                        }
                        player.Start = false;
                    }
                }

            }
            // Delete all pipes
            pipesList.RemoveAll(p => p.Deleted);

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
                List<Pipe> pipesList, SpriteFont spriteFont) {

            // Drawing variables 
            Rectangle backgroundPosition = new Rectangle(0, 0, DefaultWidth, DefaultHeight);
            Vector2 scorePosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - FontRadius, ScoreYPosition);
            Vector2 titlePosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - TitleRadius, ScoreYPosition);
            Vector2 highScorePosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - HighScoreRadius, (graphics.PreferredBackBufferHeight / 2) + HighScoreYPositionOffset);

            // Always draw background and player
            spriteBatch.Draw(backgroundSprite, backgroundPosition, Color.White);
            player.Draw(spriteBatch);

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
            }
        }

        /**
         * Score Method: 
         *
         * increments score variables
         * when a player passes a group of pipes 
         *
         */
        public bool ScoreCheck(Player player, Pipe pipe) {
            if (player.Position.X == pipe.Position.X) {
                score += ScoreValue;
                if (score > highScore) {
                    highScore = score;
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
         */
        public bool CollisionDetected(GraphicsDeviceManager graphics, Player player, Pipe pipe) {

            // Return true if player is out of bounds on the Y position 
            if (player.Position.Y >= graphics.PreferredBackBufferHeight - GroundCollision) {
                return true;
            }


            // check if the player is in the same x area as the pipe
            bool inXArea = false;
            int pipeLeftMaximum = (int)pipe.Position.X - pipe.HalfWidth;
            int pipeRightMaximum = (int)pipe.Position.X + pipe.HalfWidth;
            int playerLeftCheck = (int)(player.Position.X + player.Radius) - CollisionOffset;
            int playerRightCheck = (int)(player.Position.X - player.Radius) + CollisionOffset;
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
                int yCollisionVal = (int)(pipe.Position.Y + pipe.HalfHeight) - CollisionOffset;
                if (player.Position.Y < yCollisionVal) {
                    return true;
                }
            }
            else if (inXArea && !abovePlayer) {
                int yCollisionVal = (int)(pipe.Position.Y - pipe.HalfHeight) + CollisionOffset;
                if (player.Position.Y > yCollisionVal) {
                    return true;
                }
            }

            // Default return value (no collision took place)            
            return false;
        }
    }
}
