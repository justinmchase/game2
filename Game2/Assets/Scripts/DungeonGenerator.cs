using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class DungeonGenerator : MonoBehaviour {

	public GameObject[] Rooms;

	public int MaxRooms = 20;

	private System.Random rand = new System.Random();

	public Texture2D debugImage;

	private HashSet<Vector3Int> occupiedSpaces = new HashSet<Vector3Int>();

	// Use this for initialization
	void Start () {

		this.debugImage = new Texture2D(128, 128);
		this.GenerateLevel();

	}

	public void GenerateLevel()
	{
		var openDoors = new List<RoomConnectorBehavior>();
		var roomPrefab = Rooms[0];

		var room = GameObject.Instantiate(roomPrefab);
		openDoors.AddRange(room.GetComponentsInChildren<RoomConnectorBehavior>());
		this.Occupy(room, Vector3.zero);

		int limit = MaxRooms;

		var stubs = this.Rooms.ToList();
		stubs.Remove(this.Rooms[0]);

		while(openDoors.Any() && limit > -30000) {
			limit--;
			var door = openDoors.ElementAt(rand.Next(openDoors.Count));
			if(limit > 0){
				GrowDungeon(door, this.Rooms, openDoors);
			}else{
				GrowDungeon(door, stubs, openDoors);
			}
		}

		this.DumpToTexture(occupiedSpaces);
	}

	void GrowDungeon(RoomConnectorBehavior door, IEnumerable<GameObject> rooms, List<RoomConnectorBehavior> openDoors) {

		var matchingConnectors = rooms.SelectMany(r =>{
				return r
					.GetComponentsInChildren<RoomConnectorBehavior>()
					.Where(c => c.Direction == (ConnectorDirection)(((int)door.Direction + 2) % 4));
		}).ToList();

		GameObject nextRoom = null;
		while (nextRoom == null && matchingConnectors.Any()) {
			var selectedConnector = matchingConnectors[this.rand.Next(matchingConnectors.Count())];
			var s = selectedConnector.transform.parent.gameObject;
			var d = door.transform.position - selectedConnector.transform.localPosition;
			if (this.CheckOccupancy(s, d)) {		
				nextRoom = GameObject.Instantiate(s);
				nextRoom.transform.position = d;
				this.Occupy(s, d);
			} else {
				matchingConnectors.Remove(selectedConnector);
			}
		}

		if (nextRoom == null) {
			Debug.Log("No room for you!");
			return; // should not ever happen!
		}

		//we need to remove all connectors in the new room that match with all connectors in the open list
		// as well as all the open doors in the open doors list that match
		var newDoors = nextRoom.GetComponentsInChildren<RoomConnectorBehavior>().ToList();//.Where(c => c.transform.localPosition != selectedConnector.transform.localPosition);
		//openDoors.AddRange(newDoors);
		UpdateOpenDoors(openDoors, newDoors);
	}

	void UpdateOpenDoors(List<RoomConnectorBehavior> openDoors, List<RoomConnectorBehavior> newDoors){
		foreach(var adoor in openDoors.ToList()){
			foreach(var bdoor in newDoors.ToList()){
				if(adoor.transform.position == bdoor.transform.position){
					openDoors.Remove(adoor);
					newDoors.Remove(bdoor);
				}
			}
		}
		openDoors.AddRange(newDoors);
	}

	// Update is called once per frame
	void Update () {
		
	}

	bool CheckOccupancy(GameObject room, Vector3 destination) {
		foreach(var tilemap in room.GetComponentsInChildren<Tilemap>()) {
			var area = tilemap.cellBounds;
			for (var x = area.xMin; x < area.xMax; x++)
			for (var y = area.yMin; y < area.yMax; y++)
			for (var z = area.zMin; z < area.zMax; z++) {
				var p = new Vector3Int(x, y, z);
				if (tilemap.HasTile(p)) {
					var tileDestination = p + Vector3Int.RoundToInt(destination);
					if (occupiedSpaces.Contains(tileDestination)) return false;
				}
			}
		}

		return true;
	}

	void Occupy(GameObject room, Vector3 destination) {
		foreach(var tilemap in room.GetComponentsInChildren<Tilemap>()) {
			var area = tilemap.cellBounds;
			for (var x = area.xMin; x < area.xMax; x++)
			for (var y = area.yMin; y < area.yMax; y++)
			for (var z = area.zMin; z < area.zMax; z++) {
				var p = new Vector3Int(x, y, z);
				if (tilemap.HasTile(p)) {
					var tileDestination = p + Vector3Int.RoundToInt(destination);
					occupiedSpaces.Add(tileDestination);
				}
			}
		}
	}

	void DumpToTexture (HashSet<Vector3Int> occupiedSpaces) {
		for (int y = 0; y < this.debugImage.height; y++)
		for (int x = 0; x < this.debugImage.width; x++)
		{
			var color = occupiedSpaces.Contains(new Vector3Int(x - (this.debugImage.width/2), y - (this.debugImage.height/2), 0)) ? Color.red : Color.white;
			this.debugImage.SetPixel(x, y, color);
		}

		this.debugImage.Apply();
	}
}
