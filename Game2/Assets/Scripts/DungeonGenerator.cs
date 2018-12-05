using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class DungeonGenerator : MonoBehaviour {

	public int Seed;
	public int[] levels;

	public int currentDungeon = 0;

	void Start() {
		if (levels == null || levels.Length == 0) {
			var rand = new System.Random(this.Seed);
			levels = new int[rand.Next(10, 20)];
			for (var i = 0; i < levels.Length; i++) {
				levels[i] = rand.Next();
			}
		}

		var levelSeed = this.levels[this.currentDungeon];
		this.GetComponent<DungeonLevelGenerator>().GenerateLevel(levelSeed, 1);
	}

	public void Next(int direction) {
		if (this.currentDungeon + direction >= this.levels.Length) return;
		if (this.currentDungeon + direction < 0) return;

		foreach(Transform child in this.transform) {
			GameObject.Destroy(child.gameObject);
		}

		this.currentDungeon += direction;
		var levelSeed = this.levels[this.currentDungeon];
		this.GetComponent<DungeonLevelGenerator>().GenerateLevel(levelSeed, direction);
	}

}
