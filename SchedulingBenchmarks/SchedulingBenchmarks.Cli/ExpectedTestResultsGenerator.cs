using SchedulingBenchmarks.Evaluators;
using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks.Cli
{
    class ExpectedTestResultsGenerator
    {
        private readonly SchedulingBenchmarkModel _result;
        private readonly Dictionary<string, EmployeeFeasibilityAggregate> _aggregates;
        private readonly FeasibilityEvaluator _feasibilityEvaluator;

        public ExpectedTestResultsGenerator(int instanceNumber)
        {
            var dto = SchedulingBenchmarkInstanceReader.FromXml(instanceNumber);
            var schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto);

            _result = SchedulerAlgorithmRunner.Run(schedulingBenchmarkModel);
            _aggregates = FeasibilityDataAggregator.GetAggregate(_result).ToDictionary(a => a.EmployeeId);
            _feasibilityEvaluator = new FeasibilityEvaluator(_result);
        }

        public static ExpectedTestResult RunForInstance(int instanceNumber) => new ExpectedTestResultsGenerator(instanceNumber).GenerateResult();

        public ExpectedTestResult GenerateResult()
        {
            var result = new ExpectedTestResult
            {
                ExpectedPenalty = OptimalityEvaluator.CalculatePenalty(_result),
                ExpectedMaxNumberOfShiftsFeasibility = ForEachEmployee(_feasibilityEvaluator.MaxNumberOfShiftsNotExceeded),
                ExpectedMinTotalMinsFeasibility = ForEachEmployee(_feasibilityEvaluator.MinTotalMinutesNotExceeded),
                ExpectedMaxTotalMinsFeasibility = ForEachEmployee(_feasibilityEvaluator.MaxTotalMinutesNotExceeded),
                ExpectedMinConsecutiveShiftsFeasibility = ForEachEmployee(_feasibilityEvaluator.MinConsecutiveShiftsNotExceeded),
                ExpectedMaxConsecutiveShiftsFeasibility = ForEachEmployee(_feasibilityEvaluator.MaxConsecutiveShiftsNotExceeded),
                ExpectedMinConsecutiveDaysOffFeasibility = ForEachEmployee(_feasibilityEvaluator.MinConsecutiveDaysOffNotExceeded),
                ExpectedMaxNumberOfWeekendsFeasibility = ForEachEmployee(_feasibilityEvaluator.MaxNumberOfWeekendsNotExceeded),
                ExpectedDayOffsFeasibility = ForEachEmployee(_feasibilityEvaluator.DayOffsRespected),
                ExpectedMinRestTimeFeasibility = ForEachEmployee(_feasibilityEvaluator.MinRestTimeRespected)
            };

            result.ExpectedFeasibility = Feasible(result);

            return result;
        }

        private bool ForEachEmployee(Func<Employee, EmployeeFeasibilityAggregate, bool> func)
        {
            return _result.Employees.AsParallel().All(e => func(e, _aggregates[e.Id]));
        }

        private bool Feasible(ExpectedTestResult result)
        {
            return result.ExpectedMaxNumberOfShiftsFeasibility
                && result.ExpectedMinTotalMinsFeasibility
                && result.ExpectedMaxTotalMinsFeasibility
                && result.ExpectedMinConsecutiveShiftsFeasibility
                && result.ExpectedMaxConsecutiveShiftsFeasibility
                && result.ExpectedMinConsecutiveDaysOffFeasibility
                && result.ExpectedMaxNumberOfWeekendsFeasibility
                && result.ExpectedDayOffsFeasibility
                && result.ExpectedMinRestTimeFeasibility;
        }
    }
}
