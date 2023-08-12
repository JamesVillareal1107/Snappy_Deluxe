using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snappy_Deluxe {

    public class Game1 : Game {

        // Constants 
        private const int DefaultWidth = 1280;
        private const int DefaultHeight = 720;
        private const int PlayerScale = 80; 
        private const int HalfWidth = DefaultWidth / 2;
        private const int HalfHeight = DefaultHeight / 2;

        // graphics devices
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Sprites 
        private Texture2D backgroundSprite;
        private Texture2D playerSprite;

        // Game Objects 
        private Player player;


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

            // Initialize game objects 
            player = new Player(_graphics);
            
            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here 
            backgroundSprite = Content.Load<Texture2D>("Sprites/Background");
            playerSprite = Content.Load<Texture2D>("Sprites/Birds/Chicken/PNG/Chicken 1");
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here  
            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here  

            // Variables for draw operation  
            Rectangle backgroundPos = new Rectangle(0, 0, DefaultWidth, DefaultHeight);
            Rectangle playerPos = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Radius, player.Radius);

            // Draw operation
            _spriteBatch.Begin();

            // Draw background
            _spriteBatch.Draw(backgroundSprite, backgroundPos, Color.White);

            // Draw player 
            _spriteBatch.Draw(playerSprite, playerPos, Color.White);

            _spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}