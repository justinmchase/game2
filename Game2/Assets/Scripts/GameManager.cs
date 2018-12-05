﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Game
{
  public class GameManager : MonoBehaviour
  {
    public float Scale = 1.0f;
    public GameObject player;
    public GameObject interactiveObject;
    public GameObject UI;
    
    public ViewportRect viewPort;

    public int nextId = 0;

    public static GameManager current;

    public List<int> DungeonLevelSeeds = new List<int>();
  

    public void Awake(){
      Debug.Log("set gamemanager");
      GameManager.current = this;
      State.instance.InitState();
    }

    public void Update()
    {
      this.HandleCameraMovement();
      this.HandlePlayerInput();

    }

    public void HandleCameraMovement() {
      if(player != null){
        var h = Camera.main.pixelHeight;
        var w = Camera.main.pixelWidth;

        Camera.main.orthographicSize = h / (16 * this.Scale);

        Camera.main.transform.position = player.transform.position + new Vector3(0, 0, -1000);

        var radiusY = Camera.main.orthographicSize + 2;
        var radiusX = radiusY * Camera.main.aspect + 2;
        var centerX = Camera.main.transform.position.x;
        var centerY = Camera.main.transform.position.y;

        this.viewPort = ViewportRect.FromCenterRadius(centerX, centerY, radiusX, radiusY);
      }
    }

    public void HandlePlayerInput() {
      if(player != null) {
        player.GetComponent<CreatureBehavior>().IsRunning = Input.GetKey(KeyCode.LeftShift);
        player.GetComponent<CreatureBehavior>().MoveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        var cb = player.GetComponent<CreatureBehavior>();
        if (Input.GetKeyDown(KeyCode.Return)) {
          cb.Interact(true);
        }
        if (Input.GetKeyUp(KeyCode.Return)) {
          cb.Interact(false);
        }
      }
    }
  }
}
