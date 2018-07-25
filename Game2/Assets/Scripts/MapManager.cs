
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

public class MapManager : MonoBehaviour {

	public int Width = 100;
	public int Height = 100;

	private int[,] _map;
	public Sprite[] Sprites;

	public ViewportRect previousRect;
	public Material Material;

	// Use this for initialization
	void Start () {

		int n = this.Sprites.Length;
		var r = new System.Random();

		_map = new int[Width, Height];
		for (int y = 0; y < Height; y++)
		{
			for (int x = 0; x < Width; x++)
			{
				_map[y, x] = r.Next(0, n);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {

		var radiusY = Camera.main.orthographicSize;
		var radiusX = radiusY * Camera.main.aspect;
		
		var centerX = Camera.main.transform.position.x;
		var centerY = Camera.main.transform.position.y;

		var currentRect = new ViewportRect(){
			startX = (int)Math.Max(0, centerX - radiusX),
			startY = (int)Math.Max(0, centerY - radiusY),
			endX = (int)Math.Min(Width, centerX + radiusX + 2),
			endY = (int)Math.Min(Height, centerY + radiusY + 2)
		};

		foreach (Transform child in this.transform) {
			if(!currentRect.PointInRect((int)child.position.x, (int)child.position.y)){
				Destroy(child.gameObject);
			}
		}

		// Debug.Log("{0} {1} {2} {3}")
		for (int y = currentRect.startY; y < currentRect.endY; y++)
		for (int x = currentRect.startX; x < currentRect.endX; x++)
		{
			if(this.previousRect == null || !this.previousRect.PointInRect(x, y)){
				var g = new GameObject(string.Format("tile_{0}_{1}", x, y));
				g.transform.parent = this.transform;
				g.transform.position = new Vector3(x, y, 0);

				SpriteRenderer r = g.AddComponent<SpriteRenderer>();
				r.sprite = this.Sprites[this._map[y, x]];
				r.material = this.Material;
			}
		}

		this.previousRect = currentRect;
	}
}
