using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator
{
    public static class DungeonGeneratorExtensions
    {
        public static Vector3Int GetExitPosition(this RoomConnectorBehavior door, Vector3 destination)
        {
            Vector3 p = door.transform.localPosition + destination;
            p += door.Direction == ConnectorDirection.N ? new Vector3(0, 0.5f, 0) :
                     door.Direction == ConnectorDirection.E ? new Vector3(0.5f, 0, 0) :
                     door.Direction == ConnectorDirection.S ? new Vector3(0, -0.5f, 0) :
                     door.Direction == ConnectorDirection.W ? new Vector3(-0.5f, 0, 0) :
                     new Vector3(0, 0, 0);

            return Vector3Int.RoundToInt(p);
        }

        public static Vector3Int GetEnterPosition(this RoomConnectorBehavior door, Vector3 destination)
        {
            Vector3 p = door.transform.localPosition + destination;
            p += door.Direction == ConnectorDirection.N ? new Vector3(0, -0.5f, 0) :
                     door.Direction == ConnectorDirection.E ? new Vector3(-0.5f, 0, 0) :
                     door.Direction == ConnectorDirection.S ? new Vector3(0, 0.5f, 0) :
                     door.Direction == ConnectorDirection.W ? new Vector3(0.5f, 0, 0) :
                     new Vector3(0, 0, 0);

            return Vector3Int.RoundToInt(p);
        }

        public static T GetRandomItem<T>(this IEnumerable<T> items, System.Random rand) {

            return items.ElementAt(rand.Next(items.Count()));
        }
    }
}
