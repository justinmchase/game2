using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class AttackHitboxBehavior : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("hitbox entered");
		var cb = collider.GetComponent<CreatureBehavior>();
		if(cb == null) return;
		var me = this.transform.parent.GetComponent<CreatureBehavior>();
		if(me == null) return;

		if(cb.Team != me.Team){ //todo maybe increment a count of number of targets in range
			me.GetComponent<Animator>().SetBool("EnemyInRange", true);
			me.AttackFocus = cb.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		var cb = collider.GetComponent<CreatureBehavior>();
		if(cb == null) return;
		var me = this.transform.parent.GetComponent<CreatureBehavior>();
		if(me == null) return;

		me.AttackFocus = null;
		me.GetComponent<Animator>().SetBool("EnemyInRange", false);
	}
}
