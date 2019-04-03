using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarBehavior : MonoBehaviour {

    public CreatureBehavior creature;
    private Slider slider;

	// Use this for initialization
	void Start () {
        this.slider = this.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.creature != null && this.slider != null)
        {
            this.slider.maxValue = creature.MaxMana;
            this.slider.value = creature.Mana;
        }
	}
}
