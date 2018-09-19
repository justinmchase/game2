using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class SpawnerBehavior : MonoBehaviour {

	public enum eSpawnType {GameStart, None};

	public GameObject Prefab;
	public List<GameObject> SpawnedObjects = new List<GameObject>();
	public bool IsPlayer;

	public eSpawnType SpawnType;

	public void Spawn(){
		if(Prefab != null){
			var p = GameObject.Instantiate(Prefab);
			p.transform.position = this.transform.position;
			p.transform.rotation = this.transform.rotation;

			if(this.IsPlayer){
				GameManager.current.player = p;
			}
		}
	}

	void Start () {
		if(this.SpawnType == eSpawnType.GameStart){
			this.Spawn();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
