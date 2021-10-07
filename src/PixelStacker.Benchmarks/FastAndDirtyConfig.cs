using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Benchmarks
{
    public class FastAndDirtyConfig : ManualConfig
    {
        public FastAndDirtyConfig()
        {
            Add(DefaultConfig.Instance); // *** add default loggers, reporters etc? ***

            AddJob(Job.ShortRun
                .WithLaunchCount(1)     // benchmark process will be launched only once
                .WithWarmupCount(1)     // 3 warmup iteration
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.ColdStart)
            );
        }
    }
}
