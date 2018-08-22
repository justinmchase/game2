using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Pyxel
{
  [Serializable]
  public class PyxelLayer
  {
    public string name; // "lid"
    public bool soloed; // false
    public int alpha; // 255
    public string blendMode; // "normal"
    public bool hidden; // false
    public bool muted; // false
    public PyxelTileRefs tileRefs;
  }

  [Serializable]
  public class PyxelLayers : IEnumerable<PyxelLayer>
  {
    public PyxelLayer _0;
    public PyxelLayer _1;
    public PyxelLayer _2;
    public PyxelLayer _3;
    public PyxelLayer _4;
    public PyxelLayer _5;
    public PyxelLayer _6;
    public PyxelLayer _7;
    public PyxelLayer _8;
    public PyxelLayer _9;
    public PyxelLayer _10;
    public PyxelLayer _11;
    public PyxelLayer _12;
    public PyxelLayer _13;
    public PyxelLayer _14;
    public PyxelLayer _15;

    private static bool IsValid(PyxelLayer layer)
    {
      return layer != null && layer.name != null;
    }

    public IEnumerable<PyxelLayer> GetLayers()
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

    public IEnumerator<PyxelLayer> GetEnumerator()
    {
      return this.GetLayers().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }
}
