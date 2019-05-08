using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeylineTool : Tool {

    public GameObject LeylinePrefab;
    public GameObject ConnectorPrefab;

    private GameObject LeylineContainer;
    private GameObject PreviousLeyline;
    private Vector3 PreviousMousePosition;

    private Vector3 NotSet = new Vector3(float.NaN, float.NaN, float.NaN);

    private void Awake()
    {
        PreviousMousePosition = NotSet;
        this.LeylineContainer = GameObject.Find("Leylines");
    }

    void PlaceLeyline(Vector3 position)
    {
        position.z = 0;
        position.x = Mathf.Floor(position.x) + 0.5f;
        position.y = Mathf.Floor(position.y) + 0.5f;

        var name = string.Format("l_{0}_{1}", position.x, position.y);

        var ll = GameObject.Find(name);

        if (ll == null)
        {
            ll = GameObject.Instantiate(LeylinePrefab);
            ll.transform.position = position;
            ll.transform.parent = LeylineContainer.transform;
            ll.name = name;
        }

        if (this.PreviousLeyline != null && this.PreviousLeyline != ll)
        {
            var src = this.PreviousLeyline;

            var dst = ll;

            if (string.Compare(src.name, ll.name) > 0)
            {
                var tmp = src;
                src = dst;
                dst = tmp;
            }

            var cn = string.Format("c_{0}_{1}", src.name, dst.name);

            var connector = GameObject.Find(cn);
            if (connector == null)
            {
                connector = GameObject.Instantiate(ConnectorPrefab);
                connector.name = cn;
                connector.transform.parent = LeylineContainer.transform;
                connector.GetComponent<LeylineConnectorBehavior>().Src = src.GetComponent<LeylineBehavior>();
                connector.GetComponent<LeylineConnectorBehavior>().Dst = dst.GetComponent<LeylineBehavior>();
                connector.GetComponent<LeylineConnectorBehavior>().Orient();

                src.GetComponent<LeylineBehavior>().Connectors.Add(connector);
                dst.GetComponent<LeylineBehavior>().Connectors.Add(connector);


            }
        }


        this.PreviousLeyline = ll;
    }

    // Update is called once per frame
    void Update () {


        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && PreviousMousePosition != NotSet)
        {

            position.z = 0;
            position.x = Mathf.Floor(position.x) + 0.5f;
            position.y = Mathf.Floor(position.y) + 0.5f;

            var distance = Vector3.Distance(position, PreviousMousePosition);


            var xmotion = new Vector3(position.x, PreviousMousePosition.y, PreviousMousePosition.z);

            while (PreviousMousePosition != xmotion)
            {
                PreviousMousePosition = Vector3.MoveTowards(PreviousMousePosition, xmotion, 1f);
                PlaceLeyline(PreviousMousePosition);
            }

            var ymotion = new Vector3(PreviousMousePosition.x, position.y, PreviousMousePosition.z);

            while (PreviousMousePosition != ymotion)
            {
                PreviousMousePosition = Vector3.MoveTowards(PreviousMousePosition, ymotion, 1f);
                PlaceLeyline(PreviousMousePosition);
            }

        }

        PreviousMousePosition = position;

        if (Input.GetMouseButtonUp(0))
        {
            this.PreviousLeyline = null;
        }

    }
}
