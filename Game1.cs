using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace Snappy_Deluxe {

    public class Game1 : Game {

        // Constants 
        private const int DefaultWidth = 1280;
        private const int DefaultHeight = 720;
        private const float SongVolume = 0.1f;

        // graphics devices
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Sprites & SpriteFonts
        private Texture2D backgroundSprite;
        private Texture2D chickenSprite;
        private Texture2D henSprite;
        private Texture2D penguinSprite;
        private Texture2D pigeonSprite;
        private Texture2D pipeDownSprite;
        private Texture2D pipeUpSprite;
        private Texture2D groundSprite;
        private SpriteFont scoreSpriteFont;
        private SpriteFont messageSpriteFont;

        // Game Objects
        private Player player; 
        private Random spawnOffset; 
        private List<Pipe> pipesList;
        private List<Texture2D> sprites;
        private GameManager gameState;
        private GroundManager grounds;

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
            chickenSprite = Content.Load<Texture2D>("Sprites/Birds/Chicken/PNG/Chicken 1");
            henSprite = Content.Load<Texture2D>("Sprites/Birds/Hen/PNG/Hen 1");
            penguinSprite = Content.Load<Texture2D>("Sprites/Birds/Penguin/PNG/Penguin 1");
            pigeonSprite = Content.Load<Texture2D>("Sprites/Birds/Pigeon/PNG/Pigeon 1");
            pipeDownSprite = Content.Load<Texture2D>("Sprites/Obstacle Pipe/Pipe Down");
            pipeUpSprite = Content.Load<Texture2D>("Sprites/Obstacle Pipe/Pipe Up");  
            groundSprite = Content.Load<Texture2D>("Sprites/Platform/Platform"); 
            scoreSpriteFont = Content.Load<SpriteFont>("Sprites/UI/Score Font");
            messageSpriteFont = Content.Load<SpriteFont>("Sprites/UI/MessageFont");
            
            // Add all skins to the game 
            sprites = new List<Texture2D>();
            sprites.Add(chickenSprite);
            sprites.Add(henSprite);
            sprites.Add(penguinSprite);
            sprites.Add(pigeonSprite);

            // Initialize game objects 
            player = new Player(_graphics, sprites);
            spawnOffset = new Random();
            pipesList = new List<Pipe>();
            gameState = new GameManager();
            grounds = new GroundManager(groundSprite); 
            
            // Add sounds/music into the game
            Sounds.jumpSound = Content.Load<SoundEffect>("Sounds/BirdFlap");
            Sounds.deathSound = Content.Load<SoundEffect>("Sounds/RetroExplosion7");
            Sounds.scoreSound = Content.Load<SoundEffect>("Sounds/SnappyScore");
            Sounds.birdDying = Content.Load<SoundEffect>("Sounds/BirdDying");
            Sounds.backgroundMusic = Content.Load<Song>("Sounds/SnappyUltimateBackgroundMusic");

            // Play the background music on startup and for the whole game  
            MediaPlayer.Volume = SongVolume;
            MediaPlayer.Play(Sounds.backgroundMusic);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here 
            gameState.Update(gameTime, _graphics, player, spawnOffset, pipesList, pipeUpSprite, pipeDownSprite);
            grounds.Update(gameTime, _graphics, gameState);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here 
            _spriteBatch.Begin(); 
            gameState.Draw(_graphics,_spriteBatch, backgroundSprite, player, spawnOffset, pipesList, scoreSpriteFont, messageSpriteFont); 
            grounds.Draw(_spriteBatch, scoreSpriteFont); 
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
