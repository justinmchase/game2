using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public struct Mana
{
    public int Red;
    public int Blue;
    public int Yellow;

    public int Orange;
    public int Purple;
    public int Green;

    public int Brown;

    public int White;
    public int Black;

    public bool X;

    public bool IsNaM()
    {
        if(this.GetMana().All(i => i >= 0))
        {
            if (this.X) return true;
            return false;
        }

        if (this.GetMana().All(i => i <= 0))
        {
            return false;
        }

        return true;
    }

    public IEnumerable<int> GetMana()
    {
        yield return Red;
        yield return Blue;
        yield return Yellow;

        yield return Orange;
        yield return Purple;
        yield return Green;

        yield return Brown;

        yield return White;
        yield return Black;
    }  


    public static Mana operator -(Mana a)
    {
        return new Mana()
        {
            Red = -a.Red ,
            Blue = -a.Blue,
            Yellow = -a.Yellow,
            Orange = -a.Orange,
            Purple = -a.Purple,
            Green = -a.Green,
            Brown = -a.Brown,
            White = -a.White,
            Black = -a.Black,
        };
    }

    public static Mana operator +(Mana a, Mana b)
    {
        return new Mana()
        {
            Red = a.Red + b.Red,
            Blue = a.Blue + b.Blue,
            Yellow = a.Yellow + b.Yellow,
            Orange = a.Orange + b.Orange,
            Purple = a.Purple + b.Purple,
            Green = a.Green + b.Green,
            Brown = a.Brown + b.Brown,
            White = a.White + b.White,
            Black = a.Black + b.Black
        };
    }

    public static bool operator ==(Mana e0, Mana e1)
    {
        return e0.Equals(e1);
    }
    public static bool operator !=(Mana e0, Mana e1)
    {
        return !e0.Equals(e1);
    }

    public static bool operator >(Mana e0, Mana e1)
    {
        return e0.GetMana().Sum() > e1.GetMana().Sum();
    }

    public static bool operator <(Mana e0, Mana e1)
    {
        return !(e0 >= e1);
    }

    public static bool operator >=(Mana e0, Mana e1)
    {
        return e0.GetMana().Sum() >= e1.GetMana().Sum();
    }

    public static bool operator <=(Mana e0, Mana e1)
    {
        return !(e0 > e1);
    }

    public static Mana Zero = new Mana();
}
