
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Maps;

namespace Game
{
	public class ViewportRect {
		public int startX;
		public int startY;
		public int endX;
		public int endY;

		public bool PointInRect(int x, int y)
		{
			return (x >= startX &&
				x < endX &&
				y >= startY &&
				y < endY ) ;
		}
		
		public bool PointInRect(float x, float y)
		{
			return (x >= startX &&
				x < endX &&
				y >= startY &&
				y < endY ) ;
		}
	}

	public class MapManager : MonoBehaviour {

		public Sprite[] Sprites;

		public ViewportRect previousRect;
		public Material Material;

		private Map Map;

		// Use this for initialization
		void Start () {
			var generator = this.GetComponent<MapGenerator>();
			this.Map = generator.Generate(this.Sprites.Length);
		}
		
		// Update is called once per frame
		void Update () {

			Camera.main.transform.position = this.Map.player.position;
			var radiusY = Camera.main.orthographicSize;
			var radiusX = radiusY * Camera.main.aspect;
			var centerX = Camera.main.transform.position.x;
			var centerY = Camera.main.transform.position.y;

			var currentRect = new ViewportRect() {
				startX = (int)Math.Max(0, centerX - radiusX),
				startY = (int)Math.Max(0, centerY - radiusY),
				endX = (int)Math.Min(this.Map.width, centerX + radiusX + 2),
				endY = (int)Math.Min(this.Map.height, centerY + radiusY + 2)
			};

			foreach (Transform child in this.transform) {
				if (!currentRect.PointInRect((int)child.position.x, (int)child.position.y)){
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
					r.sprite = this.Sprites[this.Map.tiles[y, x]];
					r.material = this.Material;
				}
			}

			for (int n = 0; n < this.Map.actors.Length; n++)
			{
				var actor = this.Map.actors[n];
				var x = actor.position.x;
				var y = actor.position.y;
				if ((this.previousRect == null || !this.previousRect.PointInRect(x, y)) && currentRect.PointInRect(x, y)) {
					var prefab = actor.prefab;
					var g = GameObject.Instantiate(prefab);
					g.name = string.Format("prop_{0}", n);
					g.transform.parent = this.transform;
					g.transform.position = new Vector3(x, y, y);
					g.SendMessage("Model", actor, SendMessageOptions.DontRequireReceiver);
				}
			}

			this.previousRect = currentRect;
		}
	}
}
