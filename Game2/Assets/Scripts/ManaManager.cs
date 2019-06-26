using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface ILeyline
{
    Vector3Int p0 { get; }
    Vector3Int p1 { get; }
}

public class LeylineNetwork
{
    private static int NextId = 0;

    private int id = NextId++;
    private List<LeylineNode> nodes = new List<LeylineNode>();
    public void Merge(LeylineNetwork n)
    {
        this.nodes.AddRange(n.nodes);
    }

    public void Add(LeylineNode node)
    {
        this.nodes.Add(node);
    }

    public void Tick()
    {
        var p = new Mana();
        foreach (var n in this.nodes)
        {
            p += n.Potential;
        }

        var charged = p == Mana.Zero;
        Debug.Log(string.Format("LeylineNetwork Charge: {0} {1}", this.id, charged));

        foreach (var n in this.nodes)
        {
            n.Charged = charged;
        }
    }
}

public class ManaManager : MonoBehaviour, ITickable
{
    private Dictionary<Vector3Int, LeylineNetwork> networks = new Dictionary<Vector3Int, LeylineNetwork>();

	void Start ()
    {
        GameManager.current.Register(this);
    }

    public void Tick()
    {
        //--
        // For each network
        //  Calculate the potential of all points
        //  if potential is 0 then set all points to Charged = true
        //  else set them all to Charged = false
        //  

        foreach(var n in this.networks.Values)
        {
            n.Tick();
        }
    }

    public void AddLeyline(ILeyline leyline)
    {
        Debug.Log(string.Format("Leyline: {0}, {1}", leyline.p0, leyline.p1));

        LeylineNetwork n0, n1;
        this.networks.TryGetValue(leyline.p0, out n0);
        this.networks.TryGetValue(leyline.p1, out n1);

        if (n0 == null && n1 == null)
        {

            Debug.Log(string.Format("LeylineNetwork: n0 {0}, n1 {1}", leyline.p0, leyline.p1));
            n0 = n1 = new LeylineNetwork();
            this.networks.Add(leyline.p0, n0);
            this.networks.Add(leyline.p1, n1);
        }

        if (n0 != null && n1 == null)
        {
            Debug.Log(string.Format("LeylineNetwork: n0 {0}", leyline.p0));
            this.networks.Add(leyline.p1, n0);
        }

        if (n0 == null && n1 != null)
        {
            Debug.Log(string.Format("LeylineNetwork: n1 {0}", leyline.p1));
            this.networks.Add(leyline.p0, n1);
        }

        if (n0 != null & n1 != null)
        {
            if (n0 == n1)
            {
                // no-op
                Debug.Log(string.Format("LeylineNetwork: noop"));
            }
            else
            {
                Debug.Log(string.Format("LeylineNetworks: n0 {0}, n1 {1}", leyline.p0, leyline.p1));
                // combine n0 and n1
                // and use n0
                n0.Merge(n1);
                foreach(var t in this.networks.ToArray())
                {
                    if (t.Value == n1)
                    {
                        this.networks[t.Key] = n0;
                    }
                }
                // n1 is no longer ref'd
            }
        }
    }

    public void AddNode(LeylineNode node)
    {
        Debug.Log(string.Format("LeylineNode: {0}", node.transform.position.ToInt()));

        var p = Vector3Int.FloorToInt(node.transform.position);
        LeylineNetwork n0;
        if (!this.networks.TryGetValue(p, out n0))
        {
            n0 = new LeylineNetwork();
            this.networks.Add(p, n0);
        }
        n0.Add(node);
    }
    
    private void CalculateNetworks()
    {
        // Flood fill
        // 1. Build grah connection map
        // 2. repeat until there are no open connections
        // 3. For each provider find which network it is in
        // 4. For each consumer find which network it is in
    }
}
