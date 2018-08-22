using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Pyxel
{
  [Serializable]
  public class PyxelTileRef
  {
    public int index;
    public bool flipX;
    public int rot;
  }

  [Serializable]
  public class PyxelTileRefs : IEnumerable<PyxelTileRef>
  {
    public PyxelTileRef _0;
    public PyxelTileRef _1;
    public PyxelTileRef _2;
    public PyxelTileRef _3;
    public PyxelTileRef _4;
    public PyxelTileRef _5;
    public PyxelTileRef _6;
    public PyxelTileRef _7;
    public PyxelTileRef _8;
    public PyxelTileRef _9;
    public PyxelTileRef _10;
    public PyxelTileRef _11;
    public PyxelTileRef _12;
    public PyxelTileRef _13;
    public PyxelTileRef _14;
    public PyxelTileRef _15;

    private static bool IsValid(PyxelTileRef tileRef)
    {
      return tileRef != null; // && tileRef.name != null;
    }

    public IEnumerable<PyxelTileRef> GetTileRefs()
    {
      if (IsValid(_0)) yield return _0;
      if (IsValid(_1)) yield return _1;
      if (IsValid(_2)) yield return _2;
      if (IsValid(_3)) yield return _3;
      if (IsValid(_4)) yield return _4;
      if (IsValid(_5)) yield return _5;
      if (IsValid(_6)) yield return _6;
      if (IsValid(_7)) yield return _7;
      if (IsValid(_8)) yield return _8;
      if (IsValid(_9)) yield return _9;
      if (IsValid(_10)) yield return _10;
      if (IsValid(_11)) yield return _11;
      if (IsValid(_12)) yield return _12;
      if (IsValid(_13)) yield return _13;
      if (IsValid(_14)) yield return _14;
      if (IsValid(_15)) yield return _15;
    }

    public IEnumerator<PyxelTileRef> GetEnumerator()
    {
      return this.GetTileRefs().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }
}
