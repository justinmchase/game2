using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.DungeonGenerator
{
    public struct DungeonGeneratorContext
    {
        public System.Random rand;

        public Texture2D debugImage;

        public GameObject Level;
        public HashSet<Vector3Int> OpenTiles;
        public HashSet<Vector3Int> ObstructedTiles;
        public List<RoomConnectorBehavior> openDoors;
        public HashSet<Vector3Int> occupiedSpaces;
        public HashSet<Vector3Int> occupiedDoorSpaces;
        public HashSet<Vector3Int> misses;

        public static DungeonGeneratorContext Create(int seed, GameObject level)
        {
            return new DungeonGeneratorContext()
            {
                OpenTiles = new HashSet<Vector3Int>(),
                ObstructedTiles = new HashSet<Vector3Int>(),
                openDoors = new List<RoomConnectorBehavior>(),
                occupiedSpaces = new HashSet<Vector3Int>(),
                occupiedDoorSpaces = new HashSet<Vector3Int>(),
                misses = new HashSet<Vector3Int>(),
                rand = new System.Random(seed),
                debugImage = new Texture2D(128, 128),
                Level = level
            };
        }

        bool CheckOccupancy(GameObject room, Vector3 destination, int maxRoomConnections = int.MaxValue)
        {

            var newDoors = room.GetComponentsInChildren<RoomConnectorBehavior>().ToList();

            int connectionCount = 0;
            foreach (var door in newDoors)
            {

                var p = door.GetExitPosition(destination);
                if (occupiedDoorSpaces.Contains(p))
                {
                    connectionCount++;
                    if (connectionCount > maxRoomConnections)
                    {
                        return false;
                    }

                    continue;
                }

                if (!occupiedSpaces.Contains(p)) continue;

                return false;
            }

            foreach (var tilemap in room.GetComponentsInChildren<Tilemap>())
            {
                var sortingLayerName = tilemap.GetComponent<TilemapRenderer>().sortingLayerName;
                if (sortingLayerName == "Caps" || sortingLayerName == "Overlay")
                {
                    continue;
                }

                var area = tilemap.cellBounds;
                for (var x = area.xMin; x < area.xMax; x++)
                    for (var y = area.yMin; y < area.yMax; y++)
                        for (var z = area.zMin; z < area.zMax; z++)
                        {
                            var p = new Vector3Int(x, y, z);
                            if (tilemap.HasTile(p))
                            {
                                var tileDestination = p + Vector3Int.RoundToInt(destination);
                                if (occupiedSpaces.Contains(tileDestination))
                                {
                                    return false;
                                }

                                if (occupiedDoorSpaces.Contains(tileDestination))
                                {
                                    var existingDoor = openDoors.First(od => od.GetExitPosition(od.transform.parent.transform.position) == tileDestination);
                                    var newDoor = newDoors
                                        .Where(d => d.Tag == existingDoor.Tag)
                                        .Any(d => d.GetEnterPosition(destination) == tileDestination);

                                    if (!newDoor)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
            }

            return true;
        }

        void DumpToTexture(HashSet<Vector3Int> occupiedSpaces)
        {
            for (int y = 0; y < this.debugImage.height; y++)
                for (int x = 0; x < this.debugImage.width; x++)
                {
                    var color = Color.white;
                    if (occupiedSpaces.Contains(new Vector3Int(x - (this.debugImage.width / 2), y - (this.debugImage.height / 2), 0))) color = Color.red;
                    if (occupiedDoorSpaces.Contains(new Vector3Int(x - (this.debugImage.width / 2), y - (this.debugImage.height / 2), 0))) color = Color.blue;
                    if (misses.Contains(new Vector3Int(x - (this.debugImage.width / 2), y - (this.debugImage.height / 2), 0))) color = Color.green;
                    this.debugImage.SetPixel(x, y, color);
                }

            this.debugImage.Apply();
        }

        void UpdateOpenDoors(List<RoomConnectorBehavior> newDoors)
        {
            foreach (var adoor in this.openDoors.ToList())
                foreach (var bdoor in newDoors.ToList())
                {
                    if (adoor.transform.position == bdoor.transform.position)
                    {
                        this.openDoors.Remove(adoor);
                        newDoors.Remove(bdoor);
                    }
                }

            this.openDoors.AddRange(newDoors);
            foreach (var door in newDoors)
            {
                this.occupiedDoorSpaces.Add(door.GetEnterPosition(door.transform.parent.transform.position));
                this.occupiedDoorSpaces.Add(door.GetExitPosition(door.transform.parent.transform.position));
            }
        }

        void Occupy(GameObject room, Vector3 destination)
        {
            foreach (var tilemap in room.GetComponentsInChildren<Tilemap>())
            {
                var area = tilemap.cellBounds;
                var type = tilemap.GetComponent<TilemapRenderer>().sortingLayerName;
                for (var x = area.xMin; x < area.xMax; x++)
                    for (var y = area.yMin; y < area.yMax; y++)
                        for (var z = area.zMin; z < area.zMax; z++)
                        {
                            var p = new Vector3Int(x, y, z);
                            if (tilemap.HasTile(p))
                            {
                                var tileDestination = p + Vector3Int.RoundToInt(destination);
                                occupiedSpaces.Add(tileDestination);

                                switch (type)
                                {
                                    case "Floor":
                                        this.OpenTiles.Add(tileDestination);
                                        break;
                                    case "Walls":
                                        this.ObstructedTiles.Add(tileDestination);
                                        break;
                                }
                            }
                        }
            }
        }

        public bool StartDungeon(IEnumerable<GameObject> rooms)
        {
            var room = GameObject.Instantiate(rooms.GetRandomItem(this.rand));
            room.SetActive(true);
            room.transform.parent = this.Level.transform;
            this.UpdateOpenDoors(room.GetComponentsInChildren<RoomConnectorBehavior>().ToList());
            this.Occupy(room, Vector3.zero);
            return true;
        }

        public bool GrowDungeon(RoomConnectorBehavior door, IEnumerable<GameObject> rooms, int maxRoomConnections = int.MaxValue)
        {

            var matchingConnectors = rooms.SelectMany(r =>
            {
                return r
                    .GetComponentsInChildren<RoomConnectorBehavior>()
                    .Where(c => c.Direction == (ConnectorDirection)(((int)door.Direction + 2) % 4))
                    .Where(c => c.Tag == door.Tag);
            }).ToList();

            var originalMatchingConnetors = matchingConnectors.ToArray();

            GameObject nextRoom = null;
            while (nextRoom == null && matchingConnectors.Any())
            {
                var selectedConnector = matchingConnectors[this.rand.Next(matchingConnectors.Count())];
                var s = selectedConnector.transform.parent.gameObject;
                var d = door.transform.position - selectedConnector.transform.localPosition;
                if (this.CheckOccupancy(s, d, maxRoomConnections))
                {
                    nextRoom = GameObject.Instantiate(s);
                    nextRoom.SetActive(true);
                    nextRoom.transform.position = d;
                    nextRoom.transform.parent = this.Level.transform;

                    var newDoors = nextRoom.GetComponentsInChildren<RoomConnectorBehavior>().ToList();
                    this.UpdateOpenDoors(newDoors);
                    this.Occupy(s, d);
                }
                else
                {
                    matchingConnectors.Remove(selectedConnector);
                }
            }

            if (nextRoom == null)
            {
                door.transform.localScale = new Vector3(2, 2, 2);
                foreach (var missedConnector in originalMatchingConnetors)
                {
                    var d = door.transform.position - missedConnector.transform.localPosition;
                    var p = missedConnector.GetExitPosition(d);
                    this.misses.Add(p);
                }
                return false;
            }

            return true;
        }

    }
}
