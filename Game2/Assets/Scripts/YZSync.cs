using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YZSync : MonoBehaviour {

	public void Update()
	{
    this.transform.position = new Vector3(
      this.transform.position.x,
      this.transform.position.y,
      this.transform.position.y);
	}
}
