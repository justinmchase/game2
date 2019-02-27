using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator
{
    public enum PipelineStepType
    {
        Cap,
        Grow,
        Start
    }

    [Serializable]
    public class PipelineStepData
    {
        public string Name;
        public PipelineStepType PipelineStepType;
        public int MaxRooms;
        public GameObject[] Rooms;

        public IDungeonGeneratorPipelineStep ToStep()
        {
            switch (this.PipelineStepType)
            {
                case PipelineStepType.Cap:
                    return new CapPipelineStep(this.Rooms);
                case PipelineStepType.Grow:
                    return new GrowRoomsPipelineStep(this.MaxRooms, this.Rooms);
                case PipelineStepType.Start:
                    return new StartRoomPipelineStep(this.Rooms);
                default:
                    throw new Exception("unknown step type");
         
            }
        }

    }
}
