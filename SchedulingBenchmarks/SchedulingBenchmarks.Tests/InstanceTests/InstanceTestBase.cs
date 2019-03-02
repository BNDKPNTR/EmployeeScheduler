using SchedulingBenchmarks.Cli;
using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SchedulingBenchmarks.Tests.InstanceTests
{
    public abstract class InstanceTestBase
    {
        private readonly SchedulingBenchmarkModel _result;

        public abstract int InstanceNumber { get; }
        public abstract bool ExpectedFeasibility { get; }
        public abstract int ExpectedPenalty { get; }

        public InstanceTestBase()
        {
            var dto = SchedulingBenchmarkInstanceReader.FromXml(InstanceNumber);
            var schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto);

            _result = SchedulerAlgorithmRunner.Run(schedulingBenchmarkModel);
        }

        [Fact]
        public void AssertFeasibility()
        {
            var (actualFeasibility, _) = FeasibilityEvaluator.Feasible(_result);

            Assert.Equal(ExpectedFeasibility, actualFeasibility);
        }

        [Fact]
        public void AssertOptimality()
        {
            var actualPenalty = OptimalityEvaluator.CalculatePenalty(_result);

            Assert.Equal(ExpectedPenalty, actualPenalty);
        }
    }
}
