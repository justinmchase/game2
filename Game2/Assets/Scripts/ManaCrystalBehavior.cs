using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCrystalBehavior : MonoBehaviour {

    public bool active = false;

    public void Tick()
    {
        if (this.active)
        {
            GameManager.current.player.GetComponent<CreatureBehavior>().Mana += 10;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
        this.active = true;
    }

    public void Deactivate()
    {
        this.active = false;
    }
}
