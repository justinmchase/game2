﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class DungeonLevelGenerator : MonoBehaviour {
	public GameObject[] StartRooms;
	public GameObject[] GrowRooms;
	public GameObject[] EndRooms;
	public GameObject[] FillRooms;
	public GameObject[] Caps;
	public int MaxStartRooms = 1;
	public int MaxGrowRooms = 5;
	public int MaxEndRooms = 1;
	public int MaxFillRooms = 0;

	private System.Random rand;

	public Texture2D debugImage;

	public GameObject Level;

	private HashSet<Vector3Int> occupiedSpaces = new HashSet<Vector3Int>();

	private HashSet<Vector3Int> occupiedDoorSpaces = new HashSet<Vector3Int>();

	private HashSet<Vector3Int> misses = new HashSet<Vector3Int>();

	public void GenerateLevel(int seed, int direction)
	{
		this.rand = new System.Random(seed);
		this.debugImage = new Texture2D(128, 128);
		this.Level = new GameObject();
		this.Level.transform.parent = this.transform;
		this.Level.name = "Level - " + seed;

		var openDoors = new List<RoomConnectorBehavior>();
		this.occupiedSpaces.Clear();
		this.occupiedDoorSpaces.Clear();
		this.misses.Clear();
		try
		{
			var bail = 0;
			var i = 0;
			GrowDungeon(null, this.StartRooms, openDoors);

			i = 0;
			while(i < this.MaxGrowRooms && bail < 10000) {
				bail++;
				var door = openDoors.ElementAt(rand.Next(openDoors.Count));
				if(GrowDungeon(door, this.GrowRooms, openDoors, 1)) i++;
			}

			i = 0;
			while(i < this.MaxEndRooms && bail < 10000) {
				bail++;
			  var door = openDoors.ElementAt(rand.Next(openDoors.Count));
				if(GrowDungeon(door, this.EndRooms, openDoors, 1)) i++;
			}

			while(openDoors.Any() && bail < 10000) {
				bail++;
				var door = openDoors.ElementAt(rand.Next(openDoors.Count));
				GrowDungeon(door, this.Caps, openDoors);
			}

			var spawner = this.Level.GetComponentsInChildren<StairsBehavior>().First(s => s.Direction == -direction);
			spawner.Entered = true;
			GameManager.current.player.transform.position = spawner.transform.position;
		}
		finally
		{
			this.DumpToTexture(occupiedSpaces);
		}
	}

	bool GrowDungeon(RoomConnectorBehavior door, IEnumerable<GameObject> rooms, List<RoomConnectorBehavior> openDoors, int maxRoomConnections = int.MaxValue) {

		if(door == null){
			var room = GameObject.Instantiate(rooms.First());
			room.transform.parent = this.Level.transform;
			this.UpdateOpenDoors(openDoors, room.GetComponentsInChildren<RoomConnectorBehavior>().ToList());
			this.Occupy(room, Vector3.zero);
			return true;
		}

		var roomsArray = rooms.ToArray();
		var matchingConnectors = rooms.SelectMany(r =>{
				return r
					.GetComponentsInChildren<RoomConnectorBehavior>()
					.Where(c => c.Direction == (ConnectorDirection)(((int)door.Direction + 2) % 4));
		}).ToList();

		var originalMatchingConnetors = matchingConnectors.ToArray();

		GameObject nextRoom = null;
		while (nextRoom == null && matchingConnectors.Any()) {
			var selectedConnector = matchingConnectors[this.rand.Next(matchingConnectors.Count())];
			var s = selectedConnector.transform.parent.gameObject;
			var d = door.transform.position - selectedConnector.transform.localPosition;
			if (this.CheckOccupancy(s, d, maxRoomConnections)) {
				nextRoom = GameObject.Instantiate(s);
				nextRoom.transform.position = d;
				nextRoom.transform.parent = this.Level.transform;
				
				var newDoors = nextRoom.GetComponentsInChildren<RoomConnectorBehavior>().ToList();
				this.UpdateOpenDoors(openDoors, newDoors);
				this.Occupy(s, d);
			} else {
				matchingConnectors.Remove(selectedConnector);
			}
		}

		if (nextRoom == null) {
			door.transform.localScale = new Vector3(2, 2, 2);
			foreach(var missedConnector in originalMatchingConnetors) {
				var d = door.transform.position - missedConnector.transform.localPosition;
				var p = this.GetDoorExitPosition(missedConnector, d);
				this.misses.Add(p);
			}
			return false;
		}

		return true;
	}

	void UpdateOpenDoors(List<RoomConnectorBehavior> openDoors, List<RoomConnectorBehavior> newDoors){
		foreach(var adoor in openDoors.ToList())
		foreach(var bdoor in newDoors.ToList()) {
			if(adoor.transform.position == bdoor.transform.position){
				openDoors.Remove(adoor);
				newDoors.Remove(bdoor);
			}
		}

		openDoors.AddRange(newDoors);
		foreach(var door in newDoors){
			this.occupiedDoorSpaces.Add(this.GetDoorEnterPosition(door, door.transform.parent.transform.position));
			this.occupiedDoorSpaces.Add(this.GetDoorExitPosition(door, door.transform.parent.transform.position));
		}
	}


	Vector3Int GetDoorExitPosition(RoomConnectorBehavior door, Vector3 destination){
		Vector3 p = door.transform.localPosition + destination;
		p += door.Direction == ConnectorDirection.N ? new Vector3(0,    0.5f, 0) :
				 door.Direction == ConnectorDirection.E ? new Vector3(0.5f, 0, 0) :
				 door.Direction == ConnectorDirection.S ? new Vector3(0,   -0.5f, 0) :
				 door.Direction == ConnectorDirection.W ? new Vector3(-0.5f, 0, 0) :
				 new Vector3(0, 0, 0);

		return Vector3Int.RoundToInt(p);
	}

	Vector3Int GetDoorEnterPosition(RoomConnectorBehavior door, Vector3 destination){
		Vector3 p = door.transform.localPosition + destination;
		p += door.Direction == ConnectorDirection.N ? new Vector3(0,    -0.5f, 0) :
				 door.Direction == ConnectorDirection.E ? new Vector3(-0.5f, 0, 0) :
				 door.Direction == ConnectorDirection.S ? new Vector3(0,   0.5f, 0) :
				 door.Direction == ConnectorDirection.W ? new Vector3(0.5f, 0, 0) :
				 new Vector3(0, 0, 0);

		return Vector3Int.RoundToInt(p);
	}

	bool CheckOccupancy(GameObject room, Vector3 destination, int maxRoomConnections = int.MaxValue) {

		var newDoors = room.GetComponentsInChildren<RoomConnectorBehavior>().ToList();

		int connectionCount = 0;
		foreach(var door in newDoors) {
			
			var p = this.GetDoorExitPosition(door, destination);
			if (occupiedDoorSpaces.Contains(p)){
				connectionCount++;
				if(connectionCount > maxRoomConnections){
					return false;
				}

				continue;
			} 

			if (!occupiedSpaces.Contains(p)) continue;
			
			return false;
		}

		foreach(var tilemap in room.GetComponentsInChildren<Tilemap>()) {
			var area = tilemap.cellBounds;
			for (var x = area.xMin; x < area.xMax; x++)
			for (var y = area.yMin; y < area.yMax; y++)
			for (var z = area.zMin; z < area.zMax; z++) {
				var p = new Vector3Int(x, y, z);
				if (tilemap.HasTile(p)) {
					var tileDestination = p + Vector3Int.RoundToInt(destination);
					if (occupiedSpaces.Contains(tileDestination)) {
						return false;
					}
					if (occupiedDoorSpaces.Contains(tileDestination) && !newDoors.Any(d => this.GetDoorEnterPosition(d, destination) == tileDestination)) {
						return false;
					}
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
			var color = Color.white;
			if(occupiedSpaces.Contains(new Vector3Int(x - (this.debugImage.width/2), y - (this.debugImage.height/2), 0))) color = Color.red;
			if(occupiedDoorSpaces.Contains(new Vector3Int(x - (this.debugImage.width/2), y - (this.debugImage.height/2), 0))) color = Color.blue;
			if(misses.Contains(new Vector3Int(x - (this.debugImage.width/2), y - (this.debugImage.height/2), 0))) color = Color.green;
			this.debugImage.SetPixel(x, y, color);
		}

		this.debugImage.Apply();
	}
}
