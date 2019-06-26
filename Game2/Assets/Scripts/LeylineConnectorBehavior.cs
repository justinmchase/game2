using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeylineConnectorBehavior : MonoBehaviour, ILeyline
{

    public LeylineBehavior Src;
    public LeylineBehavior Dst;

    private Direction Direction;

    public Vector3Int p0
    {
        get
        {
            return this.Src.transform.position.ToInt();
        }
    }

    public Vector3Int p1
    {
        get { return this.Dst.transform.position.ToInt(); }
    }

    private void Awake()
    {
        //this.Orient();
    }

    private void Start()
    {
        var mm = GameManager.current.GetComponent<ManaManager>();
        mm.AddLeyline(this);
    }

    public void Orient()
    {
        if (Dst == null || Src == null) return;

        this.Direction = (Dst.transform.position - Src.transform.position).ToDirection();

        this.transform.position = (Src.transform.position + Dst.transform.position) / 2.0f;
        
        switch (this.Direction)
        {
            case Direction.N:
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.S:
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.E:
                this.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.W:
                this.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
        }
    }
}
