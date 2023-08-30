﻿// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Snappy_Deluxe {
    internal class Ground {

        // Constants 
        private const int DefaultWidth = 1280;
        private const int DefaultHeight = 96;
        private const int DefaultGroundRadius = 640;
        private const int HalfHeight = 24;
        private const int DefaultSpeed = 300; 
        
        // static variables  
        public static int height = DefaultHeight;
        public static int width = DefaultWidth;
        public static int radius = DefaultGroundRadius;
        
        // Instance Variables  
        private Vector2 position;
        private int speed;
        private Texture2D sprite;
        private bool isRemoved;

        // Constructors and properties
        public Ground(Vector2 position, Texture2D sprite) {
            this.position = position; 
            this.speed = DefaultSpeed;
            this.sprite = sprite;
            this.isRemoved = false;
        }

        public bool IsRemoved {
            get { return isRemoved; }
            set { isRemoved = value; }
        }

        public Vector2 Position {
            get { return position; } 
            set { position = value; }
        }

        // Methods 
        public void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            position.X -= speed * deltaTime;
        } 
        
        /**
         * Draw Method:
         *
         * Draws ground object at its given position
         *
         * @param: spriteBatch
         * @return: N/A
         */
        public void Draw(SpriteBatch spriteBatch) {
            Rectangle groundPosition = new Rectangle((int)position.X-radius,(int)position.Y-HalfHeight, width, height);
            spriteBatch.Draw(sprite, groundPosition, Color.White);
        }
    }
}
