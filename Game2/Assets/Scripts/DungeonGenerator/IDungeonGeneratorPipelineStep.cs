using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DungeonGenerator
{
    public interface IDungeonGeneratorPipelineStep
    {
        void Grow(DungeonGeneratorContext ctx);
    }
}
