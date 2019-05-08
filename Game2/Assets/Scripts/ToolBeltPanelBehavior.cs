using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBeltPanelBehavior : MonoBehaviour {

    public ToolBeltBehavior Player;
    public GameObject ToolButton;
    public Transform[] Tools;
    public int NumSlots;
    private int ActiveTool;

	// Use this for initialization
	void Start ()
    {
		for(int i = 0; i < Player.Tools.Length; i++)
        {
            int j = i;
            var button = GameObject.Instantiate(this.ToolButton);
            button.transform.SetParent(this.transform, false);
            button.transform.Find("Button").GetComponent<Button>().image.sprite = Player.Tools[i].GetComponent<Tool>().ToolbeltIcon;
            button.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => Player.UpdateCurrentTool(j));
        }

        this.UpdateCurrentTool();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.ActiveTool != Player.CurrentTool) this.UpdateCurrentTool();
	}

    private void UpdateCurrentTool()
    {
        this.ActiveTool = this.Player.CurrentTool;
        var i = 0;
        foreach(Transform child in this.transform)
        {
            child.Find("SelectionIndicator").gameObject.SetActive(i == this.ActiveTool);
            i++;
        }
    }
}
