using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRotationBehavior : MonoBehaviour {

	private System.Random random;
	private Animator animator;
	private float dt = 0.0f;

	public int Animations = 100;
	public int Animation = -1;

	// Use this for initialization
	void Start () {
		this.random = new System.Random();
		this.animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		this.dt += Time.deltaTime;
		if (dt >= 1) {
			this.Animation = this.random.Next(0, this.Animations);
			this.animator.SetInteger("Animation", this.Animation);
			this.dt = 0.0f;
		}
	}
}
