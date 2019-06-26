using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public float Scale = 1.0f;
    public GameObject player;
    public GameObject interactiveObject;
    public GameObject UI;
    public GameObject Dungeon;

    public ViewportRect viewPort;
    private List<ITickable> tickables = new List<ITickable>();

    public int nextId = 0;

    public static GameManager current;

    public DungeonLevelGenerator Level
    {
        get { return this.Dungeon.GetComponent<DungeonLevelGenerator>(); }
    }

    public void Register(ITickable tickable)
    {
        if (!this.tickables.Contains(tickable))
        {
            this.tickables.Add(tickable);
        }
    }

    public void Awake()
    {
        if (GameManager.current == null)
        {
            GameManager.current = this;
        }
        else if (GameManager.current != this)
        {
            // Only 1 GameManger per scene.
            GameObject.Destroy(this);
        }
    }

    public void Start()
    {
        this.StartCoroutine(this.UpdateCircuits());
    }

    public void Update()
    {
        this.HandleCameraMovement();
        this.HandlePlayerInput();
    }

    private const int PIXELS_PER_UNIT = 16;

    public void HandleCameraMovement()
    {
        if (player != null)
        {
            var h = Camera.main.pixelHeight;
            var w = Camera.main.pixelWidth;

            Camera.main.orthographicSize = h / (PIXELS_PER_UNIT * this.Scale);

            var target = player.transform.position + new Vector3(0, 0, -1000);
            var current = Camera.main.transform.position;

            var velocity = target - current;
            Camera.main.transform.position = Vector3.SmoothDamp(current, target, ref velocity, Time.deltaTime);
        }
    }

    public void HandlePlayerInput()
    {
        if (player != null)
        {
            player.GetComponent<CreatureBehavior>().IsRunning = Input.GetKey(KeyCode.LeftShift);
            player.GetComponent<CreatureBehavior>().MoveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        }
    }

    public IEnumerator UpdateCircuits()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            this.tickables.RemoveAll(t => t == null);

            foreach (var tickable in this.tickables)
            {
                tickable.Tick();
            }
        }
    }
}
