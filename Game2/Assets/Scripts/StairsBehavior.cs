using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StairsBehavior : MonoBehaviour {
  public int Direction;
  public bool Entered = false;

  void Start() {
    Debug.Log("Stairs Start: " + this.Entered);
  }
  
  public void OnTriggerEnter2D(Collider2D col) {
    Debug.Log("Stairs Entered: " + this.Entered);
    if (!this.Entered) {
      this.Entered = true;
      this.GetComponentsInParent<DungeonGenerator>().First().Next(this.Direction);
    }
  }

  public void OnTriggerExit2D(Collider2D col) {
    this.Entered = false;
  }

}
