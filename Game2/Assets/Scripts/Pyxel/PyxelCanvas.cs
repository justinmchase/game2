using System;

namespace Game.Pyxel
{
  [Serializable]
  public class PyxelCanvas
  {
    public int width;
    public int height;
    public int tileWidth;
    public int tileHeight;
    public int numLayers;
    public PyxelLayers layers;
  }
}