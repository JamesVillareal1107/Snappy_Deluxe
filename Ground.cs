// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Snappy_Deluxe {
    internal class Ground {

        // Constants 
        private const int DefaultWidth = 1280;
        private const int DefaultHeight = 96;
        private const int GroundRadius = 640;
        private const int HalfHeight = 24;

        // Instance Variables  
        private Vector2 position;
        private int speed;
        private Texture2D sprite;

        // Constructors and properties
        public Ground(Vector2 position, int speed, Texture2D sprite) {
            this.position = position; 
            this.speed = speed;
            this.sprite = sprite;
        }

        // Methods 
        public void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            position.X -= speed * deltaTime;
        } 

        public void Draw(SpriteBatch spriteBatch) {
            Rectangle groundPosition = new Rectangle((int)position.X-GroundRadius,(int)position.Y-HalfHeight, DefaultWidth, DefaultHeight);
            spriteBatch.Draw(sprite, groundPosition, Color.White);
        }
    }
}
