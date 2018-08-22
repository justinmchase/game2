using System;

namespace Game.Pyxel
{
  [Serializable]
  public class PyxelTileset
  {
    public int numTiles; // 8
    public int tileHeight; // 32
    public int tileWidth; // 32
    public bool fixedWidth; // true
    public int tilesWide; // 8
  }
}
