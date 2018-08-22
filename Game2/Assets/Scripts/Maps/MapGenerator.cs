using UnityEngine;

namespace Game.Maps
{
  public abstract class MapGenerator : MonoBehaviour
  {
    public int Width = 1000;
    public int Height = 1000;
    public abstract Map Generate(int tiles);
  }
}