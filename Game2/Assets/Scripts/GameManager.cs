using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float Scale = 1.0f;

	public void Update()
	{	
		var h = Camera.main.pixelHeight;
		var w = Camera.main.pixelWidth;
		
		Camera.main.orthographicSize = h / (16 * this.Scale);
	}
}
