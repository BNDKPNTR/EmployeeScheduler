using BenchmarkDotNet.Attributes;
using SchedulingBenchmarks.Dto;
using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks.Cli
{
    public class PerfBenchmark
    {
        private SchedulingPeriod _dto;
        private SchedulingBenchmarkModel _schedulingBenchmarkModel;

        [ParamsSource(nameof(GetInstanceNumbers))]
        public int Instance { get; set; }

        [Benchmark]
        public void Execute()
        {
            SchedulerAlgorithmRunner.Run(_schedulingBenchmarkModel);
        }

        public IEnumerable<int> GetInstanceNumbers() => Enumerable.Range(1, 24);

        [GlobalSetup]
        public void GlobalSetup()
        {
            _dto = SchedulingBenchmarkInstanceReader.FromXml(Instance);
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(_dto);
        }
    }
}
