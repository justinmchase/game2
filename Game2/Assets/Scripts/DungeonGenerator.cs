using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour {

	public GameObject[] Rooms;
	public GameObject Floor;
	public GameObject Walls;
	int Width = 10;
	int Height = 10;
	private System.Random rand;

	// Use this for initialization
	void Start () {

		rand = new System.Random();

		var floor  = Floor.GetComponent<Tilemap>();
		var walls = Walls.GetComponent<Tilemap>();

		floor.origin = new Vector3Int(0, 0, 0);
		floor.size = new Vector3Int(1000, 1000, 1);

		walls.origin = new Vector3Int(0, 0, 0);
		walls.size = new Vector3Int(1000, 1000, 1);
		
		// var maxRooms = rand.Next(3, 16);
		// this.DrawRoom(new Vector3Int(-5, -5, 0), 10, 10);
		this.GenerateLevel();

		floor.CompressBounds();
		walls.CompressBounds();
	}

	public void GenerateLevel()
	{
		var room = Rooms[0];
		var roomBehavior = room.GetComponent<RoomBehavior>();
		var roomFloor = roomBehavior.Floor;
		var roomWalls = roomBehavior.Walls;

		var start = new Vector3Int(0, 0, 0);
		CopyTiles(roomFloor.GetComponent<Tilemap>(), Floor.GetComponent<Tilemap>(), start);
		CopyTiles(roomWalls.GetComponent<Tilemap>(), Walls.GetComponent<Tilemap>(), start);

		var another = new Vector3Int(15, 2, 0);
		CopyTiles(roomFloor.GetComponent<Tilemap>(), Floor.GetComponent<Tilemap>(), another);
		CopyTiles(roomWalls.GetComponent<Tilemap>(), Walls.GetComponent<Tilemap>(), another);
	}

	public void CopyTiles(Tilemap from, Tilemap to, Vector3Int location)
	{
		var offset = from.origin - to.origin;
		var bounds = from.cellBounds;
		var tiles = from.GetTilesBlock(bounds);
		var area = new BoundsInt(location + offset, bounds.size);
		to.SetTilesBlock(area, tiles);
	}
	
	// public void DrawRoom(Vector3Int pos, int width, int height){

	// 	var floor  = Floor.GetComponent<Tilemap>();
	// 	var walls = Walls.GetComponent<Tilemap>();

	// 	var bounds = new BoundsInt(pos, new Vector3Int(width, height, 1));
	// 	TileBase[] wallTiles = new TileBase[bounds.size.x * bounds.size.y * bounds.size.z];
	//   TileBase[] floorTiles = new TileBase[bounds.size.x * bounds.size.y * bounds.size.z];
	// 	for(int x = 0; x < bounds.size.x; x++){
	// 		for(int y = 0; y < bounds.size.y; y++){
	// 			TileBase t = FloorTile;
	// 			bool w = x == 0;
	// 			bool e = x == bounds.size.x - 1;
	// 			bool n = y == bounds.size.y - 1;
	// 			bool s = y == 0;

	// 			var wall
	// 				= (s && w) 	? SW_Wall
	// 				: (s && e) 	? SE_Wall 
	// 				: (n && w) 	? NW_Wall
	// 				: (n && e) 	? NE_Wall
	// 				: n					? N_Wall
	// 				: s					? S_Wall
	// 				: e					? E_Wall
	// 				: w					? W_Wall 
	// 				: null;

	// 			wallTiles[x + y * bounds.size.x] = wall;
	// 			floorTiles[x + y * bounds.size.x] = FloorTile;
	// 		}
	// 	}

	// 	walls.SetTilesBlock(bounds, wallTiles);
	// 	floor.SetTilesBlock(bounds, floorTiles);

	// }


	// Update is called once per frame
	void Update () {
		
	}
}
