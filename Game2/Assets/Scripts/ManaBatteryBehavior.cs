using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M = Mana;

public class ManaBatteryBehavior : MonoBehaviour, ITickable {

    public int Mana;
    public int MaxMana = 100;
    public int FillRate = 10;

    public bool active = false;

    public Animator animator;

    private LeylineNode input, output;

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
            }
        }

        if (this.Mana > 0 && this.output.Charged < M.Zero)
        {
            this.Mana = Math.Max(0, this.Mana - this.FillRate);
        }

        if (this.Mana < this.MaxMana && this.input.Charged >= M.Zero)
        {
            this.Mana = Math.Min(this.MaxMana, this.Mana + this.FillRate);
        }

        if (this.Mana > 0)
        {
            this.output.Potential.Blue = 1;
        }
        else
        {
            this.output.Potential.Blue = 0;
        }

        this.animator.SetFloat("Fill Percent", (float)this.Mana / (float)this.MaxMana);
    }

    private void Start()
    {
        GameManager.current.Register(this);
        var mm = GameManager.current.GetComponent<ManaManager>();
        this.input = this.transform.Find("Input").GetComponent<LeylineNode>();
        this.output = this.transform.Find("Output").GetComponent<LeylineNode>();
        mm.AddNode(this.input);
        mm.AddNode(this.output);
        this.input.Potential.Blue = -1;
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
