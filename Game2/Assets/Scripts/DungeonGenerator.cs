using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class DungeonGenerator : MonoBehaviour {

	public GameObject[] Rooms;

	private System.Random rand = new System.Random();

	// Use this for initialization
	void Start () {

		this.GenerateLevel();

	}

	public void GenerateLevel()
	{
		var openDoors = new List<RoomConnectorBehavior>();
		var roomPrefab = Rooms[0];

		var room = GameObject.Instantiate(roomPrefab);
		openDoors.AddRange(room.GetComponentsInChildren<RoomConnectorBehavior>());

		int limit = 40;

		var stubs = this.Rooms.ToList();
		stubs.Remove(this.Rooms[0]);

		while(openDoors.Any()){
			if(limit < -30000) break;
			limit--;
			Debug.Log("limit:" + limit);
			var door = openDoors.ElementAt(rand.Next(openDoors.Count));
			//openDoors.Remove(door);
			if(limit > 0){
				GrowDungeon(door, this.Rooms, openDoors);
			}else{
				GrowDungeon(door, stubs, openDoors);
			}

		}
	}

	void GrowDungeon(RoomConnectorBehavior door, IEnumerable<GameObject> rooms, List<RoomConnectorBehavior> openDoors){


		var matchingConnectors = rooms.SelectMany(r =>{
				return r
					.GetComponentsInChildren<RoomConnectorBehavior>()
					.Where(c => c.Direction == (ConnectorDirection)(((int)door.Direction + 2) % 4));
		}).ToArray();

		var selectedConnector = matchingConnectors[this.rand.Next(matchingConnectors.Length)];
		var nextRoom = GameObject.Instantiate(selectedConnector.transform.parent.gameObject);

		nextRoom.transform.position = door.transform.position - selectedConnector.transform.localPosition;

		//var connectorIndex = selectedConnector.transform.parent.children.indexOf(selectedConnector);

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
}
