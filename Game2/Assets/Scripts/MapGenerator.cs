using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop
{
  public int prop;
  public Vector2 position;
}

public class Map 
{
  public int width;
  public int height;
  public Prop[] props;
  public int[,] tiles;
}

public abstract class MapGenerator : ScriptableObject {
  public int Width = 1000;
  public int Height = 1000;
  public abstract Map Generate(
    int tiles,
    int props
  );
}

[CreateAssetMenuAttribute(fileName = "RandomMap", menuName = "Map Generators/Random Map", order = 1)]
public class RandomMapGenerator : MapGenerator
{
  public int NumRocks = 1000;

  public override Map Generate (
    int tiles,
    int props)
  {
		var r = new System.Random();
    var map = new Map
    {
      width = Width,
      height = Height,
      tiles = new int[Width, Height],
      props = new Prop[NumRocks]
    };

    // Create random tiles
		for (int y = 0; y < Height; y++)
		{
			for (int x = 0; x < Width; x++)
			{
				map.tiles[y, x] = r.Next(0, tiles);
			}
		}

    // Create random props
    for (int n = 0; n < NumRocks; n++)
    {
      map.props[n] = new Prop
      {
        prop = r.Next(0, props),
        position = new Vector2(
          r.Next(0, Width),
          r.Next(0, Height)
        )
      };
    }

    return map;
  }
}
