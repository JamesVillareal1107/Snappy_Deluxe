using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Snappy_Deluxe {
    internal static class PipeSpawner {

        // Constants 
        private static float positionX = 1574;
        private static float positionY = 360;
        private static float offset = 499;

        /** 
         * SpawnPipes method: 
         * 
         * spawns a pair of pipes at a given position when given 
         * no specific y-coordinate
         * 
         */
        public static void SpawnPipes(Texture2D topSprite, Texture2D bottomSprite, List<Pipe> pipeList) {
            pipeList.Add(new Pipe(topSprite, (int)positionX, (int)positionY-(int)offset));
            pipeList.Add(new Pipe(bottomSprite, (int)positionX, (int)positionY+(int)offset));
        } 
        
        /** 
         * SpawnPipesRandom Method: 
         * 
         * spawns a pair of pipes at a random position 
         * 
         */
        public static void SpawnPipesRandom(Texture2D topSprite, Texture2D bottomSprite, List<Pipe> pipeList, int rand) {  
            pipeList.Add(new Pipe(topSprite, (int)positionX, (int)positionY - (int)offset+rand));
            pipeList.Add(new Pipe(bottomSprite, (int)positionX, (int)positionY + (int)offset + rand));
        }
        
        
        /** 
         * SetScale method:
         * 
         * Sets values of positionX, PositionY, and Offset
         * based on the scale of the graphics device manager
         *
         * @param: graphics <GraphicsDeviceManager>
         */
        public static void SetScale(GraphicsDeviceManager graphics) {
	        positionX = graphics.PreferredBackBufferWidth * 1.23f;
	        positionY = graphics.PreferredBackBufferHeight * 0.5f;
	        offset = graphics.PreferredBackBufferHeight * 0.694f;
        }
    }
}
