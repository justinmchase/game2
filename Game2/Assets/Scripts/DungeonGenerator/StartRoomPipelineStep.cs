using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator
{
    [Serializable]
    public class StartRoomPipelineStep : IDungeonGeneratorPipelineStep
    {
        int maxNumRooms;

        public List<GameObject> Rooms = new List<GameObject>();

        public StartRoomPipelineStep(IEnumerable<GameObject> rooms)
        {
            this.Rooms = rooms.ToList();
        }

        public void Grow(DungeonGeneratorContext ctx)
        {
            ctx.StartDungeon(this.Rooms);
        }
    }
}
