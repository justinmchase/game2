using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

	public float Speed = 1.0f;

	public float IdleTime = 0.0f;
	public bool IsMoving = false;

	public bool IsMovingRight = false;
	public bool IsMovingLeft = false;

	// Update is called once per frame
	public void Update () {

		this.IdleTime += Time.deltaTime;

		var animator = this.GetComponent<Animator>();

        float moveY = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        float moveX = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;

		Vector3 velocity = new Vector3(moveX, moveY, 0);

		Vector3 moveDir = velocity;
		if(moveDir.magnitude != 0){
			moveDir.Normalize();
			this.IdleTime = 0;
		}

		if(moveDir.x < 0){
			this.transform.localScale = new Vector3(-1, 1, 1);
		}

		if(moveDir.x > 0) {
			this.transform.localScale = new Vector3(1, 1, 1);
		}

		animator.SetFloat("MoveX", moveDir.x);
		animator.SetFloat("MoveY", moveDir.y);

		this.IsMoving = moveDir.magnitude > 0;
		animator.SetBool("IsMoving", this.IsMoving);

		animator.SetFloat("IdleTime", this.IdleTime);
		
		this.transform.position += velocity;

	}
}
