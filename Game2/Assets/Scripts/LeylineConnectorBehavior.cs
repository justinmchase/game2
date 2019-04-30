using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeylineConnectorBehavior : MonoBehaviour {


    public LeylineBehavior Src;
    public LeylineBehavior Dst;

    public Direction Direction;

    public GameObject lineVisual;

    private void Awake()
    {
        

        if (this.lineVisual == null) return;

        this.Init();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {
        this.Direction = (Dst.transform.position - Src.transform.position).ToDirection();

        this.transform.position = (Src.transform.position + Dst.transform.position)/2;
        
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
