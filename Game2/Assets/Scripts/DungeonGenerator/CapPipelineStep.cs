using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator
{
    [Serializable]
    public class CapPipelineStep : IDungeonGeneratorPipelineStep
    {
        int bail = 0;

        public List<GameObject> Rooms = new List<GameObject>();

        public CapPipelineStep(IEnumerable<GameObject> rooms)
        {
            this.Rooms = rooms.ToList();
        }

        public void Grow(DungeonGeneratorContext ctx)
        {
            var i = 0;
            while (ctx.openDoors.Any() && bail < 1000)
            {
                bail++;
                var door = ctx.openDoors.GetRandomItem(ctx.rand);
                if (ctx.GrowDungeon(door, this.Rooms, 2)) i++;
            }
        }
    }
}
