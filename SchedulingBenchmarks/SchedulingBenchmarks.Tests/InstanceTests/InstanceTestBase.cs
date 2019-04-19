using Newtonsoft.Json;
using SchedulingBenchmarks.Cli;
using SchedulingBenchmarks.Evaluators;
using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace SchedulingBenchmarks.Tests.InstanceTests
{
    public abstract class InstanceTestBase
    {
        private readonly SchedulingBenchmarkModel _result;
        private readonly Dictionary<string, EmployeeFeasibilityAggregate> _aggregates;
        private readonly FeasibilityEvaluator _feasibilityEvaluator;
        private readonly ExpectedTestResult _expectedTestResult;

        public abstract int InstanceNumber { get; }

        public InstanceTestBase()
        {
            var dto = SchedulingBenchmarkInstanceReader.FromXml(InstanceNumber);
            var schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto);

            _result = SchedulerAlgorithmRunner.Run(schedulingBenchmarkModel);
            _aggregates = FeasibilityDataAggregator.GetAggregate(_result).ToDictionary(a => a.EmployeeId);
            _feasibilityEvaluator = new FeasibilityEvaluator(_result);
            _expectedTestResult = GetExpectedTestResult();
        }

        private ExpectedTestResult GetExpectedTestResult()
        {
            var jsonFilePath = Path.Combine(Environment.CurrentDirectory, "ExpectedTestResults", $"{InstanceNumber}.json");
            return JsonConvert.DeserializeObject<ExpectedTestResult>(File.ReadAllText(jsonFilePath));
        }

        [Fact]
        public void AssertOptimality()
        {
            var actualPenalty = OptimalityEvaluator.CalculatePenalty(_result);

            Assert.Equal(_expectedTestResult.ExpectedPenalty, actualPenalty);
        }

        [Fact]
        public void AssertFeasibility()
        {
            var (actualFeasibility, _) = _feasibilityEvaluator.Feasible(_aggregates.Values);
            
            Assert.Equal(_expectedTestResult.ExpectedFeasibility, actualFeasibility);
        }

        [Fact]
        public void MaxNumberOfShifts()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MaxNumberOfShiftsNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(_expectedTestResult.ExpectedMaxNumberOfShiftsFeasibility, notExceeded);
        }

        [Fact]
        public void MinTotalMins()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MinTotalMinutesNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(_expectedTestResult.ExpectedMinTotalMinsFeasibility, notExceeded);
        }

        [Fact]
        public void MaxTotalMins()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MaxTotalMinutesNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(_expectedTestResult.ExpectedMaxTotalMinsFeasibility, notExceeded);
        }

        [Fact]
        public void MinConsecutiveShifts()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MinConsecutiveShiftsNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(_expectedTestResult.ExpectedMinConsecutiveShiftsFeasibility, notExceeded);
        }

        [Fact]
        public void MaxConsecutiveShifts()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MaxConsecutiveShiftsNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(_expectedTestResult.ExpectedMaxConsecutiveShiftsFeasibility, notExceeded);
        }

        [Fact]
        public void MinConsecutiveDaysOff()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MinConsecutiveDaysOffNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(_expectedTestResult.ExpectedMinConsecutiveDaysOffFeasibility, notExceeded);
        }

        [Fact]
        public void MaxNumberOfWeekends()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MaxNumberOfWeekendsNotExceeded(e, _aggregates[e.Id]));

            Assert.Equal(_expectedTestResult.ExpectedMaxNumberOfWeekendsFeasibility, notExceeded);
        }

        [Fact]
        public void DayOffs()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.DayOffsRespected(e, _aggregates[e.Id]));

            Assert.Equal(_expectedTestResult.ExpectedDayOffsFeasibility, notExceeded);
        }

        [Fact]
        public void MinRestTime()
        {
            var notExceeded = _result.Employees.AsParallel().All(e => _feasibilityEvaluator.MinRestTimeRespected(e, _aggregates[e.Id]));

            Assert.Equal(_expectedTestResult.ExpectedMinRestTimeFeasibility, notExceeded);
        }
    }
}
