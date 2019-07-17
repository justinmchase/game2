using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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

    public LeylineNode GetOtherNode(LeylineNode n)
    {
        if (!this.nodes.Contains(n)) return null;

        return this.nodes.FirstOrDefault(x => x != n);

    }

    public void Tick()
    {
        if(this.nodes.Count() != 2)
        {
            return;
        }

        var n0 = this.nodes[0];
        var n1 = this.nodes[1];

        if (n0.Potential.IsNaM()) return;
        if (n1.Potential.IsNaM()) return;

        var p = n0.Potential + n1.Potential;// new Mana();

        if (p.IsNaM()) return;

        if (p == Mana.Zero)
        {
            n0.Charged = n1.Potential;
            n1.Charged = n0.Potential;
        }
        else if (n1.Potential.X && n0.Potential.X)
        {
            n0.Charged = Mana.Zero;
            n1.Charged = Mana.Zero;
        }
        else if (p > Mana.Zero && n0.Potential.X)
        {
            n0.Charged = n1.Potential;
            n1.Charged = -n1.Potential;
        }
        else if (p > Mana.Zero && n1.Potential.X)
        {
            n0.Charged = -n0.Potential;
            n1.Charged = n0.Potential;
        }
        else
        {
            n0.Charged = Mana.Zero;
            n1.Charged = Mana.Zero;
        }

    }
}

public class ManaManager : MonoBehaviour, ITickable
{

    HashSet<Vector3Int> Occupancy = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, LeylineNetwork> Networks = new Dictionary<Vector3Int, LeylineNetwork>();
    private List<LeylineBehavior> leylines = new List<LeylineBehavior>();

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

        foreach(var ll in this.leylines)
        {
            ll.CalculatePotential();
        }

        foreach(var network in this.Networks.Values)
        {
            network.Tick();
        }

    }

    internal bool IsOccupied(Vector3 p)
    {
        return this.Occupancy.Contains(p.ToInt());
    }

    public void Occupy(IEnumerable<Vector3> positions)
    {
        foreach(var p in positions)
        {
            this.Occupancy.Add(p.ToInt());
        }
    }
    
    public LeylineNetwork AddNode(LeylineNode node)
    {
        var position = node.transform.position.ToInt();
        var network = this.Networks.SafeGetValue(position);
        if(network == null)
        {
            network = new LeylineNetwork();
            this.Networks[position] = network;
        }

        network.Add(node);
        return network;
    }

    public void AddLeyline(LeylineBehavior leyline)
    {
        this.leylines.Add(leyline);
    }
    
    private void CalculateNetworks()
    {
        // Flood fill
        // 1. Build grah connection map
        // 2. repeat until there are no open connections
        // 3. For each provider find which network it is in
        // 4. For each consumer find which network it is in
    }

    internal LeylineNetwork GetNetwork(Vector3 position)
    {
        return this.Networks.SafeGetValue(position.ToInt());
    }
}
