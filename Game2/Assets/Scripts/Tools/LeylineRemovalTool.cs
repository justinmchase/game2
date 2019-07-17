using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeylineRemovalTool : Tool {

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update () {


        //var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //if (Input.GetMouseButton(0))
        //{
        //    var layerMask = LayerMask.GetMask("Leyline");
        //    var colliders =  Physics2D.OverlapCircleAll(position, 0.5f, layerMask);
        //    foreach(var c in colliders)
        //    {
        //        if (c.GetComponent<LeylineConnectorBehavior>())
        //        {
        //            GameObject.Destroy(c.gameObject);
        //        }

        //        var col = c.GetComponent<LeylineBehavior>();

        //        if (col != null)
        //        {
        //            foreach(var connector in col.Connectors)
        //            {
        //                GameObject.Destroy(connector);
        //            }
        //            GameObject.Destroy(col.gameObject);
        //        }
        //    }
        //}
    }
}
