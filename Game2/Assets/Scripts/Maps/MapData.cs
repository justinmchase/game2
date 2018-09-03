using System.Collections.Generic;

namespace Game.Maps
{
  public class MapData
  {
    public int width;
    public int height;
    public int[,] tiles;
    public List<Spawner> spawners = new List<Spawner>();
  }
}