using System.Collections.Generic;
using UnityEngine;

public class PathRendererBehavior : MonoBehaviour {

    public Vector3[] Path;

    public GameObject DotPrefab;
    public GameObject LinePrefab;
    public GameObject ConnectorPrefab;

    private List<GameObject> PathObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
	}

    public void Clear()
    {
        this.Path = new Vector3[0];
        this.UpdateChildren();
    }

    private void CreateDotVisual(Vector3 location)
    {
        var dot = GameObject.Instantiate(DotPrefab);
        dot.transform.position = location;
        dot.transform.parent = this.transform;
        this.PathObjects.Add(dot);
    }

    private void CreateConnectorVisual(Vector3 location)
    {
        var dot = GameObject.Instantiate(ConnectorPrefab);
        dot.transform.position = location;
        dot.transform.parent = this.transform;
        this.PathObjects.Add(dot);
    }

    private void CreateLineVisual(Vector3 start, Vector3 end)
    {
        var con = GameObject.Instantiate(LinePrefab);
        con.transform.position = (start + end) / 2.0f;
        con.transform.parent = this.transform;
        this.PathObjects.Add(con);

        switch ((end - start).ToDirection())
        {
            case Direction.N:
                con.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.S:
                con.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.E:
                con.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.W:
                con.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
        }
    }

    // Update is called once per frame
    public void UpdateChildren () {

        foreach(var child in this.PathObjects)
        {
            GameObject.Destroy(child);
        }


        if (this.Path.Length > 1)
        {
            this.CreateConnectorVisual(this.Path[0]);

            int i = 1;

            while(i < this.Path.Length - 1)
            {
                this.CreateLineVisual(this.Path[i - 1], this.Path[i]);
                this.CreateDotVisual(this.Path[i]);
                i++;
            }

            this.CreateLineVisual(this.Path[i - 1], this.Path[i]);
            this.CreateConnectorVisual(this.Path[i]);

        }

	}
}
