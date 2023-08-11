// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Snappy_Deluxe {
    internal class Player {

        // Constants 
        private const int DefaultRadius = 80;
        private const int DefaultGravitySpeed = 5;
        private const int DefaultJumpSpeed = 10;

        // Instance variables
        private int radius;
        private Vector2 position;
        private int gravitySpeed;
        private int jumpSpeed;
        private bool isDead;

        // Constructor, set everything to default values
        public Player(GraphicsDeviceManager graphics) {
            radius = DefaultRadius;
            position = new Vector2(graphics.PreferredBackBufferWidth / 2 - radius, graphics.PreferredBackBufferHeight / 2 - radius);
            gravitySpeed = DefaultGravitySpeed;
            jumpSpeed = DefaultJumpSpeed;
            isDead = false;
        }

        // Properties Getters
        public int Radius{
            get { return radius; } 
        } 
        
        public Vector2 Position {
            get { return position; }
        }

        public int GravitySpeed {
            get { return gravitySpeed; }
        }  

        public int JumpSpeed {
            get { return jumpSpeed; }
        } 

        public bool IsDead {
            get { return isDead; }
        } 

        public void Update() {

        } 

        public void jump() {

        }

    }
}
