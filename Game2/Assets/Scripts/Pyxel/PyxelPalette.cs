using System;

namespace Game.Pyxel
{
  [Serializable]
  public class PyxelPalette
  {
    public int numColors; // 62
    public int width;
    public int height;

    // public PyxelColors colors; // not used
    // e.g.
    // "colors": {
    //   "0": "ff000000",
    //   "1": null
    // }
  }
}
