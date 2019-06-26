using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCrystalBehavior : MonoBehaviour, ITickable {

    public bool active = false;

    public int FillRate = 10;

    private LeylineNode output;

    public void Tick()
    {
        if (this.active)
        {
            var player = GameManager.current.player.GetComponent<CreatureBehavior>();
            if (player.Mana < player.MaxMana)
            {
                var mana = Mathf.Min(this.FillRate, player.MaxMana - player.Mana);
                player.Mana += mana;
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
        GameManager.current.Register(this);
        var mm = GameManager.current.GetComponent<ManaManager>();
        this.output = this.transform.Find("Output").GetComponent<LeylineNode>();
        mm.AddNode(this.output);
        this.output.Potential.Blue = 1;
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
