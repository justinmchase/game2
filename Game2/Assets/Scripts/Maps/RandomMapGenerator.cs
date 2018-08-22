using UnityEngine;
using Game.Actors;

namespace Game.Maps
{
  public class RandomMapGenerator : MapGenerator
  {
    public int NumRocks = 1000;
    public GameObject RockPrefab;
    public GameObject PlayerPrefab;

    public override Map Generate(int tiles)
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
}