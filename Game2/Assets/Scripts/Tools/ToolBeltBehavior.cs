using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBeltBehavior : MonoBehaviour {

    public GameObject[] Tools;


    private GameObject _currentTool;
    public int CurrentTool;

    public void Awake()
    {
        this.UpdateCurrentTool(0);
    }

    public void UpdateCurrentTool(int i)
    {
        if (this.Tools.Length > 0)
        {
            if (_currentTool != null)
            {
                GameObject.Destroy(_currentTool);
                _currentTool = null;
            }

            var tool = (i + Tools.Length) % Tools.Length;
            _currentTool = GameObject.Instantiate(Tools[tool]);
            _currentTool.transform.SetParent(this.transform, false);
            this.CurrentTool = tool;
        }
    }

    public void Update()
    {
        var scrolly = Input.GetAxis("Mouse ScrollWheel");
        if (scrolly > 0)
        {
            this.UpdateCurrentTool(this.CurrentTool + 1);
        }
        else if (scrolly < 0)
        {
            this.UpdateCurrentTool(this.CurrentTool - 1);
        }
    }




}
