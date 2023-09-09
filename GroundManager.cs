// Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Data;

namespace Snappy_Deluxe { 
	
	internal class GroundManager {  
		

		// Instance Variables 
		private List<Ground> groundList;
		private Texture2D groundSprite;
		private bool gameStart;

		// Constructors/Properties 
		public GroundManager(Texture2D groundSprite) {
			groundList = new List<Ground>();
			this.groundSprite = groundSprite;
			this.gameStart = true;
		}

		public Texture2D GroundSprite {
			get { return groundSprite; }
			set { groundSprite = value; }
		}

		public List<Ground> GroundList {
			get { return groundList; }
			set { groundList = value; }
		}

		// Methods 
		
		/**
		 * Update Method:
		 *
		 * Runs the update behavior on all the pipes in the game
		 * and manages the amount of pipes present in the game
		 *
		 * @param: gametime <GameTime>
		 * @param: graphics <GraphicsDeviceManager>
		 * @return: N/A
		 */
		public void Update(GameTime gameTime, GraphicsDeviceManager graphics, GameManager manager) {
			  
			// If at the start of the game, spawn initial ground objects
			if (gameStart) {
				StartupInstantiation(graphics);
				gameStart = false;
			}
			
			// Update every ground object in the game 
			if (!manager.Collided) {
				foreach (Ground curr in groundList) {
					curr.Update(gameTime);
				} 
			}

			// Spawn/Despawn loop
			if (DespawnSingleGround(graphics)) {
				SpawnSingleGround(graphics);
			}
			
			// Remove all out of bounds ground objects in the game 
			groundList.RemoveAll(p => p.IsRemoved);
		}
		
		/**
		 * Draw Method:
		 *
		 * Draws every ground object in groundsList
		 *
		 * @param: spriteBatch <SpriteBatch>
		 * @return: N/A
		 */
		public void Draw(SpriteBatch spriteBatch, SpriteFont font) {
			foreach (Ground curr in groundList) {
				curr.Draw(spriteBatch);
				curr.Draw(spriteBatch);
			} 
		}
		
		/**
		 * StartupInstantiation Method:
		 *
		 * Spawns the first grounds of the first game life cycle
		 *
		 * @param: graphics <GraphicsDeviceManager>
		 * @return: N/A
		 */
		public void StartupInstantiation(GraphicsDeviceManager graphics) {
			
			// reused variables 
			int defaultSpawnX = graphics.PreferredBackBufferWidth / 2;
			int defaultSpawnY = graphics.PreferredBackBufferHeight - (Ground.height/2); 
			
			// Spawn First ground object 
			Vector2 firstPipePosition = new Vector2(defaultSpawnX, defaultSpawnY); 
			groundList.Add(new Ground(firstPipePosition,groundSprite));

			// Spawn Second ground object 
			Vector2 secondPipePosition = new Vector2(defaultSpawnX+Ground.width,defaultSpawnY);
			groundList.Add(new Ground(secondPipePosition,groundSprite));

			// Spawn third ground object 
			Vector2 thirdPipePosition = new Vector2(defaultSpawnX+((Ground.width)*2),defaultSpawnY); 
			groundList.Add(new Ground(thirdPipePosition,groundSprite));
		}
		
		/**
		 * SpawnSingleGround Method:
		 *
		 * spawns a single pipe at the appropriate position
		 *
		 * @param: graphics <GraphicsDeviceManager>
		 * @return: N/A
		 */
		public void SpawnSingleGround(GraphicsDeviceManager graphics) {
			int positionX = (graphics.PreferredBackBufferWidth / 2) + ((Ground.width) * 2);
			int positionY = graphics.PreferredBackBufferHeight - (Ground.height/2);
			Vector2 spawnPosition = new Vector2(positionX,positionY); 
			groundList.Add(new Ground(spawnPosition, groundSprite));
		}
		
		/**
		 * DespawnSingleGroundMethod:
		 *
		 * despawns a ground object when it reaches a certain point
		 *
		 * @param: graphics <GraphicsDeviceManager>
		 * @return: Boolean value representing if a deletion took place
		 */
		public bool DespawnSingleGround(GraphicsDeviceManager graphics) {
			Ground leftMost = groundList[0];
			int deletionZone = -Ground.radius;
			if (leftMost.Position.X <= deletionZone) {
				leftMost.IsRemoved = true;
				return true;
			}
			return false;
		}
	}
} 

