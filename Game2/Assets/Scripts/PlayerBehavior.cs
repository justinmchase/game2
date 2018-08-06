using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

	public float Speed = 1.0f;

	public float IdleTime = 5.0f;
	public bool IsMoving = false;

	public bool IsMovingRight = false;
	public bool IsMovingLeft = false;

	private float LastInputTime = 0f;

	// Update is called once per frame
	public void Update () {

		this.LastInputTime += Time.deltaTime;

		var animator = this.GetComponent<Animator>();

        float moveY = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        float moveX = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;

		Vector3 moveDir = new Vector3(moveX, moveY, 0);

		if(moveDir.x < 0){
			this.transform.localScale = new Vector3(-1, 1, 1);
		}

		if(moveDir.x > 0) {
			this.transform.localScale = new Vector3(1, 1, 1);
		}

		this.IsMoving = moveDir.magnitude > 0;

		animator.SetBool("IsMoving", this.IsMoving);

		if(this.LastInputTime > this.IdleTime){
			animator.SetTrigger("Idle" + Random.Range(1, 3).ToString());
			this.LastInputTime = 0;
		}

		this.transform.position += moveDir;

	}
}
