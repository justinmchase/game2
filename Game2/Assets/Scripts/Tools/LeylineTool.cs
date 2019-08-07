using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeylineTool : Tool {

    public GameObject LeylinePrefab;
    public GameObject LeylineContainer;

    private Vector3 PreviousMousePosition;

    private List<Vector3> points = new List<Vector3>();

    private void Awake()
    {
        this.LeylineContainer = GameObject.Find("Leylines");
    }

    void StartLeyline(Vector3 position)
    {
        Debug.Log("StartLeyline:" + position);

        this.points.Clear();
        this.points.Add(position);

        var pathRenderer = this.GetComponent<PathRendererBehavior>();
        pathRenderer.Path = this.points.ToArray();
        pathRenderer.UpdateChildren();
    }

    void DragLeyline(Vector3 position)
    {
        var count = this.points.Count();
        if (count >= 2
            && (points[count - 2].ToInt() == position.ToInt())){
            this.points.RemoveAt(count - 1);
        }
        else
        { 
            this.points.Add(position);
        }

        var pathRenderer = this.GetComponent<PathRendererBehavior>();
        pathRenderer.Path = this.points.ToArray();
        pathRenderer.UpdateChildren();
    }

    void EndLeyline()
    {

    }
    

    bool IsValid
    {
        get
        {

            foreach (var p in this.points)
            {
                if (GameManager.current.GetComponent<ManaManager>().IsOccupied(p))
                {
                    return false;
                }
            }

            if (this.points.Count() != this.points.Select(p => p.ToInt()).Distinct().Count())
            {
                return false;
            }

            return true;
        }
    }


    // Update is called once per frame
    void Update ()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        position.x = Mathf.Floor(position.x) + 0.5f;
        position.y = Mathf.Floor(position.y) + 0.5f;

        //..


        if (Input.GetMouseButton(0) && this.points.Count == 0)
        {
            bool isNew = true;
            //did  we click on the start or end of a line?
            var leylines = GameObject.Find("Leylines").GetComponentsInChildren<PathRendererBehavior>();
            foreach (var ll in leylines)
            {
                if (ll.Path.First() == position)
                {
                    //yes, so we are editing it
                    this.points = ll.Path.Reverse<Vector3>().ToList();

                    GameManager.current.GetComponent<ManaManager>().UnOccupy(this.points);
                    GameObject.Destroy(ll.gameObject);
                    isNew = false;

                    var pathRenderer = this.GetComponent<PathRendererBehavior>();
                    pathRenderer.Path = this.points.ToArray();
                    pathRenderer.UpdateChildren();
                }
                else if (ll.Path.Last() == position)
                {
                    //yes, so we are editing it
                    this.points = ll.Path.ToList();
                    
                    GameManager.current.GetComponent<ManaManager>().UnOccupy(this.points);
                    GameObject.Destroy(ll.gameObject);
                    isNew = false;

                    var pathRenderer = this.GetComponent<PathRendererBehavior>();
                    pathRenderer.Path = this.points.ToArray();
                    pathRenderer.UpdateChildren();
                }
            }


            if (isNew)
            {
                StartLeyline(position);
            }
        }
        else if (Input.GetMouseButton(0) && this.points.Count > 0)
        {
            Debug.Log("LeyLineDrag");
            var last = this.points[this.points.Count - 1];
            var distance = Vector3.Distance(position, last);
            var xmotion = new Vector3(position.x, last.y, 0f);


            while (Vector3.Distance(last, xmotion) >= 1.0f)
            {
                last = Vector3.MoveTowards(last, xmotion, 1f);
                DragLeyline(last);
            }

            var ymotion = new Vector3(last.x, position.y, 0f);

            while (Vector3.Distance(last, ymotion) >= 1.0f)
            {
                last = Vector3.MoveTowards(last, ymotion, 1f);
                DragLeyline(last);
            }

            if (this.IsValid)
            {
                foreach (var c in this.GetComponentsInChildren<SpriteRenderer>())
                {
                    c.color = Color.white;
                }
            }
            else
            {
                foreach (var c in this.GetComponentsInChildren<SpriteRenderer>())
                {
                    c.color = Color.red;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {

            if (this.points.Count() > 1 && this.IsValid)
            {
                // create the leyline...
                var ll = GameObject.Instantiate(this.LeylinePrefab).GetComponent<LeylineBehavior>();
                ll.transform.parent = this.LeylineContainer.transform;
                ll.GetComponent<LeylineBehavior>().SetPoints(this.points);
            }
            Debug.Log("LeyLineEnd");
  
            this.points.Clear();

            this.GetComponent<PathRendererBehavior>().Clear();
        }
    }
}
