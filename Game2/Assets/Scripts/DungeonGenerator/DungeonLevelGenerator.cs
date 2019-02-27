using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Assets.Scripts.DungeonGenerator;

public class DungeonLevelGenerator : MonoBehaviour
{

    public PipelineStepData[] Steps;

    public GameObject Level;

    public DungeonGeneratorContext ctx;

    public void GenerateLevel(int seed, int direction)
    {
        this.Level = new GameObject();
        this.Level.transform.parent = this.transform;
        this.Level.name = "Level - " + seed;

        this.ctx = DungeonGeneratorContext.Create(seed, this.Level);

        try
        {

            var pipeline = new DungeonGeneratorPipeline(this.Steps.Select(s => s.ToStep()));

            pipeline.Run(ctx);

            var spawner = this.Level
                .GetComponentsInChildren<StairsBehavior>()
                .First(s => s.Direction == -direction);
            spawner.Entered = true;
            GameManager.current.player.transform.position = spawner.transform.position;
        }
        finally
        {
            //this.DumpToTexture(occupiedSpaces);
        }
    }

   









}
