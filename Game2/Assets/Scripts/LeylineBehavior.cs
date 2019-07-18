using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class LeylineBehavior : MonoBehaviour {

    public GameObject ConnectorPrefab;
    public GameObject DotPrefab;

    public void SetPoints(IEnumerable<Vector3> points)
    {
        var renderer = this.GetComponent<PathRendererBehavior>();
        renderer.Path = points.ToArray();
        renderer.UpdateChildren();

        var mm = GameManager.current.GetComponent<ManaManager>();
        mm.Occupy(points);
        mm.AddLeyline(points.First(), points.Last());
    }

}
