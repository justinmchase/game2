using UnityEngine;
using System.Collections.Generic;

namespace Game
{

    public class Spawner{
        public string Name = "Object";
        public bool IsPlayer = false;
        public Vector2 Position;
        public GameObject Prefab;

        public List<GameObject> SpawnedObjects = new List<GameObject>();
        
    }


}