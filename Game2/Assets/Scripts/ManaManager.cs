using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public struct Leyline
{
    public Vector3Int p0;
    public Vector3Int p1;
}

public interface IManaInput
{
    void Connect(IManaOutput output);
}

public interface IManaOutput
{
    Mana[] Available { get; }
    void Consume(Mana[] mana);
}

public class ManaManager : MonoBehaviour, ITickable
{
    HashSet<Vector3Int> Occupancy = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, IManaInput> Inputs = new Dictionary<Vector3Int, IManaInput>();
    private Dictionary<Vector3Int, IManaOutput> Outputs = new Dictionary<Vector3Int, IManaOutput>();
    private List<Leyline> leylines = new List<Leyline>();

    void Start ()
    {
        GameManager.current.Register(this);
    }

    public void Tick()
    {
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

    public void UnOccupy(IEnumerable<Vector3> positions)
    {
        foreach (var p in positions)
        {
            this.Occupancy.Remove(p.ToInt());
        }
    }

    public void AddOutput(Vector3 position, IManaOutput output)
    {
        this.Outputs.Add(position.ToInt(), output);
        this.CalculateConnections();
    }

    public void AddInput(Vector3 position, IManaInput input)
    {
        this.Inputs.Add(position.ToInt(), input);
        this.CalculateConnections();
    }

    public void AddLeyline(Vector3 p0, Vector3 p1)
    {
        this.leylines.Add(new Leyline() {
            p0 = p0.ToInt(),
            p1 = p1.ToInt()
        });
        this.CalculateConnections();
    }
    
    private void CalculateConnections()
    {
        foreach(var ll in this.leylines)
        {
            var o0 = this.Outputs.SafeGetValue(ll.p0);
            var i0 = this.Inputs.SafeGetValue(ll.p1);
            if (o0 != null && i0 != null)
            {
                i0.Connect(o0);
                continue;
            }

            var i1 = this.Inputs.SafeGetValue(ll.p0);
            var o1 = this.Outputs.SafeGetValue(ll.p1);
            if (i1 != null && o1 != null)
            {
                i1.Connect(o1);
                continue;
            }
        }
    }
}
