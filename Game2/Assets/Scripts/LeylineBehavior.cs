using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeylineBehavior : MonoBehaviour {

    public GameObject ConnectorPrefab;
    public GameObject DotPrefab;
    public GameObject LeylineNodePrefab;

    public LeylineNode A;
    public LeylineNode B;

    public void SetPoints(IEnumerable<Vector3> points)
    {
        var renderer = this.GetComponent<PathRendererBehavior>();
        renderer.Path = points.ToArray();
        renderer.UpdateChildren();

        var mm = GameManager.current.GetComponent<ManaManager>();
        mm.Occupy(points);
        mm.AddLeyline(this);
        this.A = GameObject.Instantiate(this.LeylineNodePrefab).GetComponent<LeylineNode>();
        this.A.transform.position = points.First();
        this.A.transform.parent = this.transform;

        this.B = GameObject.Instantiate(this.LeylineNodePrefab).GetComponent<LeylineNode>();
        this.B.transform.position = points.Last();
        this.B.transform.parent = this.transform;

        mm.AddNode(this.A);
        mm.AddNode(this.B);
    }

   public void CalculatePotential()
   {
        var mm = GameManager.current.GetComponent<ManaManager>();
        var networkA = mm.GetNetwork(this.A.transform.position);
        var networkB = mm.GetNetwork(this.B.transform.position);

        if(networkA != null)
        {
            var other = networkA.GetOtherNode(this.A);
            if (other != null)
            {
                this.B.Potential = other.Potential;
            }
            else
            {
                this.B.Potential = Mana.Zero;
            }
        }
        else
        {
            this.B.Potential = Mana.Zero;
        }
        

        if (networkB != null)
        {
            var other = networkB.GetOtherNode(this.B);
            if (other != null)
            {
                this.A.Potential = other.Potential;
            }
            else
            {
                this.A.Potential = Mana.Zero;
            }
        }
        else
        {
            this.A.Potential = Mana.Zero;
        }
    }

}
