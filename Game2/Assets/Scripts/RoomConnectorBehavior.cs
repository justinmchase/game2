using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ConnectorDirection {
  N, E, S, W
};

public class RoomConnectorBehavior : MonoBehaviour {

  public ConnectorDirection Direction;
  public string Tag;

}
