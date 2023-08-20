using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Snappy_Deluxe {
    internal static class PipeSpawner {

        // Constants 
        private const int positionX = 1900;
        private const int positionY = 360;
        private const int offset = 500;

        /** 
         * SpawnPipes method: 
         * 
         * spawns a pair of pipes at a given position when given 
         * no specific y-coordinate
         * 
         */
        public static void SpawnPipes(Texture2D topSprite, Texture2D bottomSprite, List<Pipe> pipeList) {
            pipeList.Add(new Pipe(topSprite, positionX, positionY-offset));
            pipeList.Add(new Pipe(bottomSprite, positionX, positionY+offset));
        } 
        
        /** 
         * SpawnPipesRandom Method: 
         * 
         * spawns a pair of pipes at a random position 
         * 
         */
        public static void SpawnPipesRandom(Texture2D topSprite, Texture2D bottomSprite, List<Pipe> pipeList, int rand) {  
            pipeList.Add(new Pipe(topSprite, positionX, positionY - offset+rand));
            pipeList.Add(new Pipe(bottomSprite, positionX, positionY + offset + rand));
        }
    }
}
