
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Maps;
using System.Linq;

namespace Game
{

	public class MapManager : MonoBehaviour {

		public Sprite[] Sprites;

		public ViewportRectInt previousRect;
		public Material Material;

		private MapData MapData;

		// Use this for initialization
		void Start () {
			var generator = this.GetComponent<RandomMapGenerator>();
			this.MapData = generator.Generate(this.Sprites.Length, 1000, 1000);

			this.GetComponent<GameManager>().Spawn(this.MapData.spawners.FirstOrDefault(s => s.IsPlayer));
		}
		
		// Update is called once per frame
		public void UpdateRender () {

			var game = this.GetComponent<GameManager>();
			if(game == null) return;

			
			var currentRect = game.viewPort.ToInt().Clamp(new ViewportRectInt(){
				startX = 0,
				startY = 0,
				endX = this.MapData.width + 2,
				endY = this.MapData.height + 2
			});

			foreach (Transform child in this.transform) {
				if (!currentRect.PointInRect(child.position.x, child.position.y)){
					Destroy(child.gameObject);
				}
			}


			// Debug.Log("{0} {1} {2} {3}")
			for (int y = currentRect.startY; y < currentRect.endY; y++)
			for (int x = currentRect.startX; x < currentRect.endX; x++)
			{
				if (this.previousRect == null || !this.previousRect.PointInRect(x, y)) {
					var g = new GameObject(string.Format("tile_{0}_{1}", x, y));
					g.transform.parent = this.transform;
					g.transform.position = new Vector3(x, y, y);

					SpriteRenderer r = g.AddComponent<SpriteRenderer>();
					r.sprite = this.Sprites[this.MapData.tiles[y, x]];
					r.material = this.Material;
				}
			}

			int i = 0;
			foreach(var spawner in this.MapData.spawners)
			{
      			var x = spawner.Position.x;
				var y = spawner.Position.y;
				if ((this.previousRect == null || !this.previousRect.PointInRect(x, y)) && currentRect.PointInRect(x, y)) {
					game.Spawn(spawner);
				}
			}

			

			this.previousRect = currentRect;
		}
	}
}
