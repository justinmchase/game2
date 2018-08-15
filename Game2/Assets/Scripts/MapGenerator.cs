using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor
{
  public GameObject prefab;
  public Vector2 position;
}

public class Player : Actor
{
  public void Update()
  {
    // todo
  }
}

public class Map 
{
  public int width;
  public int height;
  public Actor[] actors;
  public Actor player;
  public int[,] tiles;
}

public abstract class MapGenerator : ScriptableObject {
  public int Width = 1000;
  public int Height = 1000;
  public abstract Map Generate(int tiles);
}

[CreateAssetMenuAttribute(fileName = "RandomMap", menuName = "Map Generators/Random Map", order = 1)]
public class RandomMapGenerator : MapGenerator
{
  public int NumRocks = 1000;
  public GameObject RockPrefab;
  public GameObject PlayerPrefab;

  public override Map Generate (int tiles)
  {
    var numActors = NumRocks + 1;
		var r = new System.Random();
    var map = new Map
    {
      width = Width,
      height = Height,
      tiles = new int[Width, Height],
      actors = new Actor[numActors]
    };

    // Create random tiles
		for (int y = 0; y < Height; y++)
		{
			for (int x = 0; x < Width; x++)
			{
				map.tiles[y, x] = r.Next(0, tiles);
			}
		}

    // Create random actor
    for (int n = 0; n < NumRocks; n++)
    {
      map.actors[n] = new Actor
      {
        prefab = RockPrefab,
        position = new Vector2(
          r.Next(0, Width),
          r.Next(0, Height)
        )
      };
    }

    var player = new Player
    {
      prefab = PlayerPrefab,
      position = new Vector2(Height / 2, Width / 2)
    };

    map.actors[numActors - 1] = player;
    map.player = player;

    return map;
  }
}
