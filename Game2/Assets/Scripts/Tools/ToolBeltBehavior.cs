using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBeltBehavior : MonoBehaviour {

    public GameObject[] Tools;

    public GameObject CurrentTool;

    public void Awake()
    {
        if (this.Tools.Length > 0)
        {
            this.CurrentTool = GameObject.Instantiate(Tools[0]);
        }
    }

}
