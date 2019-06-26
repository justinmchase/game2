using System;

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

    public static Mana Zero = new Mana();
}
