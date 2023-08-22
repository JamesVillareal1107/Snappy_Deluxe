using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Snappy_Deluxe {

    public class Game1 : Game {

        // Constants 
        private const int DefaultWidth = 1280;
        private const int DefaultHeight = 720;

        // graphics devices
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Sprites & SpriteFonts
        private Texture2D backgroundSprite;
        private Texture2D playerSprite;
        private Texture2D pipeDownSprite;
        private Texture2D pipeUpSprite; 
        private SpriteFont scoreSpriteFont;

        // Game Objects
        private Player player; 
        private Random spawnOffset; 
        private List<Pipe> pipesList;
        private GameManager gameState;

        // Constructor
        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {

            // TODO: Add your initialization logic here 

            // Default Screensize
            _graphics.PreferredBackBufferWidth = DefaultWidth;
            _graphics.PreferredBackBufferHeight = DefaultHeight; 
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here 
            backgroundSprite = Content.Load<Texture2D>("Sprites/Background");
            playerSprite = Content.Load<Texture2D>("Sprites/Birds/Chicken/PNG/Chicken 1");
            pipeDownSprite = Content.Load<Texture2D>("Sprites/Obstacle Pipe/Pipe Down");
            pipeUpSprite = Content.Load<Texture2D>("Sprites/Obstacle Pipe/Pipe Up"); 
            scoreSpriteFont = Content.Load<SpriteFont>("Sprites/UI/Score Font");

            // Initialize game objects 
            player = new Player(_graphics, playerSprite);
            spawnOffset = new Random();
            pipesList = new List<Pipe>();
            gameState = new GameManager();
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here 
            gameState.Update(gameTime, _graphics, player, spawnOffset, pipesList, pipeUpSprite, pipeDownSprite);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here 
            _spriteBatch.Begin(); 
            gameState.Draw(_graphics,_spriteBatch, backgroundSprite, player, spawnOffset, pipesList, scoreSpriteFont); 
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
