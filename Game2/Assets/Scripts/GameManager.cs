using System.Collections;
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

    public ViewportRect viewPort;

    public int nextId = 0;

    public void Update()
    {
      this.HandleCameraMovement();
      this.HandlePlayerInput();
      
      this.GetComponent<MapManager>().UpdateRender();
      
    }

    public void HandleCameraMovement() {
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

    public void HandlePlayerInput() {
      if(player != null) {
        player.GetComponent<CreatureBehavior>().IsRunning = Input.GetKey(KeyCode.LeftShift);
        player.GetComponent<CreatureBehavior>().MoveDirection = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0);

        var cb = player.GetComponent<CreatureBehavior>();
        if (Input.GetKeyDown(KeyCode.Return)) {
          cb.Interact(true);
        }
        if (Input.GetKeyUp(KeyCode.Return)) {
          cb.Interact(false);
        }
      }
    }

    public void Spawn(Spawner spawner) {
      if(spawner == null) {
        Debug.Log("Tried to spawn null spawner");
        return;
      }

      if(spawner.SpawnedObjects.Any()) {
        Debug.Log("Tried to spawn too many " + spawner.Name);
        return;
      }

      var prefab = spawner.Prefab;
      var g = GameObject.Instantiate(prefab);
      g.name = string.Format("{0}_{1}", spawner.Name, nextId++);
      g.transform.parent = this.transform;
      g.transform.position = new Vector3(spawner.Position.x, spawner.Position.y, spawner.Position.y);

      if(spawner.IsPlayer){
        this.player = g;
      }

      spawner.SpawnedObjects.Add(g);
    }
  }
}
