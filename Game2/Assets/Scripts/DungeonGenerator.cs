using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour {

	public GameObject Floor;
	public GameObject Walls;
	int Width = 10;
	int Height = 10;

	public TileBase FloorTile;

	public TileBase N_Wall;
	public TileBase S_Wall;
	public TileBase E_Wall;
	public TileBase W_Wall;

	public TileBase NW_Wall;
	public TileBase SW_Wall;
	public TileBase SE_Wall;
	public TileBase NE_Wall;

	// Use this for initialization
	void Start () {

		var floor  = Floor.GetComponent<Tilemap>();
		var walls = Walls.GetComponent<Tilemap>();

		floor.origin = new Vector3Int(0, 0, 0);
		floor.size = new Vector3Int(1000, 1000, 1);

		walls.origin = new Vector3Int(0, 0, 0);
		walls.size = new Vector3Int(1000, 1000, 1);
		
		//tm.BoxFill(new Vector3Int(0, 0, 0), FloorTile, 0, 0, 7, 10);
		this.DrawRoom(new Vector3Int(5, 5, 0), 10, 10);
		floor.CompressBounds();
		walls.CompressBounds();
	}
	
	public void DrawRoom(Vector3Int pos, int width, int height){

		var floor  = Floor.GetComponent<Tilemap>();
		var walls = Walls.GetComponent<Tilemap>();

		var bounds = new BoundsInt(pos, new Vector3Int(width, height, 1));
		TileBase[] wallTiles = new TileBase[bounds.size.x * bounds.size.y * bounds.size.z];
	  TileBase[] floorTiles = new TileBase[bounds.size.x * bounds.size.y * bounds.size.z];
		for(int x = 0; x < bounds.size.x; x++){
			for(int y = 0; y < bounds.size.y; y++){
				TileBase t = FloorTile;
				bool w = x == 0;
				bool e = x == bounds.size.x - 1;
				bool n = y == bounds.size.y - 1;
				bool s = y == 0;

				var wall
					= (s && w) 	? SW_Wall
					: (s && e) 	? SE_Wall 
					: (n && w) 	? NW_Wall
					: (n && e) 	? NE_Wall
					: n					? N_Wall
					: s					? S_Wall
					: e					? E_Wall
					: w					? W_Wall 
					: null;

				wallTiles[x + y * bounds.size.x] = wall;
				floorTiles[x + y * bounds.size.x] = FloorTile;
			}
		}

		walls.SetTilesBlock(bounds, wallTiles);
		floor.SetTilesBlock(bounds, floorTiles);

	}


	// Update is called once per frame
	void Update () {
		
	}


}
