using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class EncounterPossibility {
  public float Probibility;
  public GameObject prefab;
}

public class EncounterBehavior : MonoBehaviour {
  public EncounterPossibility[] Possibilities;

  public void Start()
  {
    var rand = new System.Random();
    var prob = rand.NextDouble();
    var last = 0.0d;

    for (var i = 0; i < this.Possibilities.Length; i++) {
      var p = this.Possibilities[i];
      if (prob < last + p.Probibility) {
        var obj = GameObject.Instantiate(p.prefab);
        obj.transform.parent = this.transform;
        obj.transform.position  = this.transform.position;
        break;
      } else {
        last += p.Probibility;
      }
    }
  }
}
