using Game.Actors;

namespace Game.Maps
{
  public class Map
  {
    public int width;
    public int height;
    public Actor[] actors;
    public Actor player;
    public int[,] tiles;
  }
}