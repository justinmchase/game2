using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManaBatteryBehavior : MonoBehaviour, IManaInput, IManaOutput, ITickable {

    public Mana Color;
    public List<Mana> Mana = new List<Mana>();
    public int MaxMana = 100;
    public int FillRate = 10;

    public bool active = false;

    public Animator animator;

    private IManaOutput output;

    public void Connect(IManaOutput output)
    {
        this.output = output;
    }

    public Mana[] Available
    {
        get
        {
            var n = Mathf.Min(this.Mana.Count, this.FillRate);
            return this.Mana.Take(n).ToArray();
        }
    }

    public void Tick()
    {
        if (this.output != null)
        {
            var n = Mathf.Min(
                this.output.Available.Where(m => m == this.Color).Count(),
                this.MaxMana - this.Mana.Count,
                this.FillRate
            );

            var mana = this.output.Available.Where(m => m == this.Color).Take(n);
            this.output.Consume(mana.ToArray());
            this.Mana.AddRange(mana);
        }
        this.animator.SetFloat("Fill Percent", (float)this.Mana.Count / (float)this.MaxMana);
    }

    void IManaOutput.Consume(Mana[] mana)
    {
        this.Mana.RemoveRange(0, mana.Length);
    }

    private void Start()
    {
        GameManager.current.Register(this);
        var mm = GameManager.current.GetComponent<ManaManager>();
        var input = this.transform.Find("Input");
        var output = this.transform.Find("Output");
        mm.AddInput(input.transform.position, this);
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
