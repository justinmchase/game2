using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator
{
    [Serializable]
    public class GrowRoomsPipelineStep : IDungeonGeneratorPipelineStep
    {
        int maxNumRooms;
        int bail = 0;

        public List<GameObject> Rooms = new List<GameObject>();

        public GrowRoomsPipelineStep(int maxNumRooms, IEnumerable<GameObject> rooms)
        {
            this.maxNumRooms = maxNumRooms;
            this.Rooms = rooms.ToList();
        }

        public void Grow(DungeonGeneratorContext ctx)
        {
            var i = 0;
            while (i < this.maxNumRooms && bail < 1000)
            {
                bail++;
                var door = ctx.openDoors.GetRandomItem(ctx.rand);
                if (ctx.GrowDungeon(door, this.Rooms, 2)) i++;
            }

            if(bail < 1000)
            {
                Debug.Log("Bail GrowRoomsPipelineStep");
            }
        }
    }
}
