using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRotationBehavior : MonoBehaviour {

	private System.Random random;
	private Animator animator;
	private float dt = 0.0f;

	public int Animations = 100;

	// Use this for initialization
	void Start () {
		this.random = new System.Random();
		this.animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		this.dt += Time.deltaTime;
		int animation = -1;
		if (dt >= 1) {
			animation = this.random.Next(0, this.Animations);
			this.animator.SetInteger("Animation", animation);
			this.dt = 0.0f;
		}

		// Debug.Log(string.Format("{0} {1}", this.dt, animation));

	}
}
