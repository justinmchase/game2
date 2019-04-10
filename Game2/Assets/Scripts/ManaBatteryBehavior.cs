using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBatteryBehavior : MonoBehaviour, ITickable {

    public int Mana;
    public int MaxMana = 100;
    public int FillRate = 10;

    public bool active = false;

    public Animator animator;

    public void Tick()
    {
        if (this.active)
        {
            var player = GameManager.current.player.GetComponent<CreatureBehavior>();
            if (player.Mana > 0 && this.Mana < this.MaxMana)
            {
                var mana = Mathf.Min(player.Mana, this.MaxMana - this.Mana, this.FillRate);
                player.Mana -= mana;
                this.Mana += mana;

                this.animator.SetFloat("Fill Percent", (float)this.Mana / (float)this.MaxMana);
            }
        }
    }

    private void Start()
    {
        GameManager.current.Register(this);   
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
