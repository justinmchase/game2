using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Pyxel
{
  [Serializable]
  public class PyxelAnimation
  {
    public int length;
    public int frameDuration;
    public string name;
    public int[] frameDurationMultipliers;
    public int baseTile;
  }

  [Serializable]
  public class PyxelAnimations : IEnumerable<PyxelAnimation>
  {
    public PyxelAnimation _0;
    public PyxelAnimation _1;
    public PyxelAnimation _2;
    public PyxelAnimation _3;
    public PyxelAnimation _4;
    public PyxelAnimation _5;
    public PyxelAnimation _6;
    public PyxelAnimation _7;
    public PyxelAnimation _8;
    public PyxelAnimation _9;
    public PyxelAnimation _10;
    public PyxelAnimation _11;
    public PyxelAnimation _12;
    public PyxelAnimation _13;
    public PyxelAnimation _14;
    public PyxelAnimation _15;

    private static bool IsValid(PyxelAnimation anim)
    {
      return anim != null && anim.name != null;
    }

    public IEnumerable<PyxelAnimation> GetAnimations()
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

    public IEnumerator<PyxelAnimation> GetEnumerator()
    {
      return this.GetAnimations().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }
}
