using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorBehavior : MonoBehaviour {


    public bool IsOpen = false;




    public void Update()
    {
        this.GetComponent<Animator>().SetBool("IsOpen", this.IsOpen);    
    }
}
