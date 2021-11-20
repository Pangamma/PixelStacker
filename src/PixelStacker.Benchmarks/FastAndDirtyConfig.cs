using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

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
                .WithIterationCount(5)
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.ColdStart)
            )
                //.WithOptions(ConfigOptions.DisableOptimizationsValidator)
                ;
        }
    }
}
