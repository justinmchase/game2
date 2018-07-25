using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

	public float Speed = 1.0f;

	// Update is called once per frame
	public void Update () {
		
        var moveY = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        var moveX = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;

		this.transform.position += new Vector3(moveX, moveY, 0);

	}
}
