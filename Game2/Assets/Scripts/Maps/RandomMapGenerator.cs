using UnityEngine;

namespace Game.Maps
{
  public class RandomMapGenerator : MonoBehaviour
  {
    public int NumRocks = 1000;
    public GameObject RockPrefab;
    public GameObject PlayerPrefab;

    public MapData Generate(int tiles, int Width, int Height)
    {
      var numActors = NumRocks + 1;
      var r = new System.Random();
      var map = new MapData
      {
        width = Width,
        height = Height,
        tiles = new int[Width, Height],
      };

      // Create random tiles
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          map.tiles[y, x] = r.Next(0, tiles);
        }
      }

      // Create random spawners
      for (int n = 0; n < NumRocks; n++)
      {
        map.spawners.Add(new Spawner(){
          Position = new Vector3(Random.value * Width, Random.value * Height, 0),
          Prefab = RockPrefab,
          Name="Rock"});
      }

      map.spawners.Add(new Spawner(){
        Position = new Vector3(Random.value * Width, Random.value * Height, 0),
        Prefab = PlayerPrefab,
        IsPlayer = true,
        Name="Player"
      });

      return map;
    }
  }
}