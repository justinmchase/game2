using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DungeonGenerator
{
    public struct DungeonGeneratorPipeline
    {
        IDungeonGeneratorPipelineStep[] steps;

        public DungeonGeneratorPipeline(IEnumerable<IDungeonGeneratorPipelineStep> steps)
        {
            this.steps = steps.ToArray();
        }

        public void Run(DungeonGeneratorContext ctx)
        {
            foreach(var step in steps)
            {
                step.Grow(ctx);
            }
        }
    }
}
