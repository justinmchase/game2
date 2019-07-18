using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCrystalBehavior : MonoBehaviour, IManaOutput {

    public bool active = false;

    public int FillRate = 1;

    public Mana Color;

    public Mana[] Available
    {
        get
        {
            return new Mana[1] { this.Color };
        }
    }

    public void Consume(Mana[] mana)
    {
    }

	// Use this for initialization
	void Start ()
    {
        //GameManager.current.Register(this);
        var mm = GameManager.current.GetComponent<ManaManager>();
        var output = this.transform.Find("Output");
        mm.AddOutput(output.transform.position, this);
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
