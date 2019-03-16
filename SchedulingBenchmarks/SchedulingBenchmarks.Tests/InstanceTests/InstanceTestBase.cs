using SchedulingBenchmarks.Cli;
using SchedulingBenchmarks.Evaluators;
using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SchedulingBenchmarks.Tests.InstanceTests
{
    public abstract class InstanceTestBase
    {
        private readonly SchedulingBenchmarkModel _result;
        private readonly Dictionary<string, EmployeeFeasibilityAggregate> _aggregates;
        private readonly FeasibilityEvaluator _feasibilityEvaluator;

        public abstract int InstanceNumber { get; }
        public abstract int ExpectedPenalty { get; }
        public abstract bool ExpectedFeasibility { get; }
        public abstract bool ExpectedMaxNumberOfShiftsFeasibility { get; }
        public abstract bool ExpectedMinTotalMinsFeasibility { get; }
        public abstract bool ExpectedMaxTotalMinsFeasibility { get; }
        public abstract bool ExpectedMinConsecutiveShiftsFeasibility { get; }
        public abstract bool ExpectedMaxConsecutiveShiftsFeasibility { get; }
        public abstract bool ExpectedMinConsecutiveDaysOffFeasibility { get; }
        public abstract bool ExpectedMaxNumberOfWeekendsFeasibility { get; }
        public abstract bool ExpectedDayOffsFeasibility { get; }
        public abstract bool ExpectedMinRestTimeFeasibility { get; }

        public InstanceTestBase()
        {
            var dto = SchedulingBenchmarkInstanceReader.FromXml(InstanceNumber);
            var schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto);

            _result = SchedulerAlgorithmRunner.Run(schedulingBenchmarkModel);
            _aggregates = FeasibilityDataAggregator.GetAggregate(_result).ToDictionary(a => a.EmployeeId);
            _feasibilityEvaluator = new FeasibilityEvaluator(_result);
        }

        [Fact]
        public void AssertOptimality()
        {
            var actualPenalty = OptimalityEvaluator.CalculatePenalty(_result);

            Assert.Equal(ExpectedPenalty, actualPenalty);
        }

        [Fact]
        public void AssertFeasibility()
        {
            var (actualFeasibility, _) = _feasibilityEvaluator.Feasible(_aggregates.Values);
            
            Assert.Equal(ExpectedFeasibility, actualFeasibility);
        }

        [Fact]
        public void MaxNumberOfShifts()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MaxNumberOfShiftsNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(ExpectedMaxNumberOfShiftsFeasibility, notExceeded);
        }

        [Fact]
        public void MinTotalMins()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MinTotalMinutesNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(ExpectedMinTotalMinsFeasibility, notExceeded);
        }

        [Fact]
        public void MaxTotalMins()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MaxTotalMinutesNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(ExpectedMaxTotalMinsFeasibility, notExceeded);
        }

        [Fact]
        public void MinConsecutiveShifts()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MinConsecutiveShiftsNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(ExpectedMinConsecutiveShiftsFeasibility, notExceeded);
        }

        [Fact]
        public void MaxConsecutiveShifts()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MaxConsecutiveShiftsNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(ExpectedMaxConsecutiveShiftsFeasibility, notExceeded);
        }

        [Fact]
        public void MinConsecutiveDaysOff()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MinConsecutiveDaysOffNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(ExpectedMinConsecutiveDaysOffFeasibility, notExceeded);
        }

        [Fact]
        public void MaxNumberOfWeekends()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MaxNumberOfWeekendsNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(ExpectedMaxNumberOfWeekendsFeasibility, notExceeded);
        }

        [Fact]
        public void DayOffs()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.DayOffsRespected(e, _aggregates[e.Id]));

            Assert.Equal(ExpectedDayOffsFeasibility, notExceeded);
        }

        [Fact]
        public void MinRestTime()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MinRestTimeRespected(e, _aggregates[e.Id]));

            Assert.Equal(ExpectedMinRestTimeFeasibility, notExceeded);
        }
    }
}
