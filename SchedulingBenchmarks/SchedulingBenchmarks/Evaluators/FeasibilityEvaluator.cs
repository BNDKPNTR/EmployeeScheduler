using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Evaluators
{
    public class FeasibilityEvaluator
    {
        private readonly SchedulingBenchmarkModel _schedulingBenchmarkModel;
        private readonly ConcurrentBag<string> _messages;

        public FeasibilityEvaluator(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));
            _messages = new ConcurrentBag<string>();
        }

        public static (bool feasible, List<string> messages) Evaluate(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            var aggregates = EvaluationDataAggregator.GetAggregate(schedulingBenchmarkModel);
            return new FeasibilityEvaluator(schedulingBenchmarkModel).Feasible(aggregates);
        }

        public (bool feasible, List<string> messages) Feasible(IEnumerable<EmployeeEvaluationAggregate> feasibilityAggregates)
        {
            var feasible = true;
            var aggregates = feasibilityAggregates.ToDictionary(a => a.EmployeeId);

            Parallel.ForEach(_schedulingBenchmarkModel.Employees, employee =>
            {
                var aggregate = aggregates[employee.Id];

                foreach (var rule in GetRules())
                {
                    if (rule(employee, aggregate) == false)
                    {
                        feasible = false;
                    }
                }
            });

            return (feasible, _messages.ToList());
        }

        private IEnumerable<Func<Employee, EmployeeEvaluationAggregate, bool>> GetRules()
        {
            return new Func<Employee, EmployeeEvaluationAggregate, bool>[]
            {
                MaxNumberOfShiftsNotExceeded,
                MinTotalMinutesNotExceeded,
                MaxTotalMinutesNotExceeded,
                MinConsecutiveShiftsNotExceeded,
                MaxConsecutiveShiftsNotExceeded,
                MinConsecutiveDaysOffNotExceeded,
                MaxNumberOfWeekendsNotExceeded,
                DayOffsRespected,
                MinRestTimeRespected
            };
        }

        public bool MaxNumberOfShiftsNotExceeded(Employee employee, EmployeeEvaluationAggregate aggregate)
        {
            var feasible = true;

            foreach (var maxShift in employee.Contract.MaxShifts)
            {
                if (aggregate.ShiftCounts.TryGetValue(maxShift.Key, out var shiftCount) && shiftCount > maxShift.Value)
                {
                    feasible = false;
                    _messages.Add($"{employee.Id} has {shiftCount} assigned shifts of type {maxShift.Key} instead of the maximum allowed {maxShift.Value}");
                }
            }

            return feasible;
        }

        public bool MinTotalMinutesNotExceeded(Employee employee, EmployeeEvaluationAggregate aggregate)
        {
            var feasible = true;

            if (aggregate.TotalWorkedMinutes < employee.Contract.MinTotalWorkTime)
            {
                feasible = false;
                _messages.Add($"{employee.Id} works {aggregate.TotalWorkedMinutes} minutes instead of the minimum required {employee.Contract.MinTotalWorkTime}");
            }

            return feasible;
        }

        public bool MaxTotalMinutesNotExceeded(Employee employee, EmployeeEvaluationAggregate aggregate)
        {
            var feasible = true;

            if (aggregate.TotalWorkedMinutes > employee.Contract.MaxTotalWorkTime)
            {
                feasible = false;
                _messages.Add($"{employee.Id} works {aggregate.TotalWorkedMinutes} minutes instead of the maximum allowed {employee.Contract.MaxTotalWorkTime}");
            }

            return feasible;
        }

        public bool MinConsecutiveShiftsNotExceeded(Employee employee, EmployeeEvaluationAggregate aggregate)
        {
            var feasible = true;

            foreach (var consecutiveShiftCount in aggregate.ConsecutiveShiftLengths)
            {
                if (ConsecutiveShiftIsNotContinuationOfPreviousOrNextSchedulePeriod(consecutiveShiftCount))
                {
                    if (consecutiveShiftCount.Length < employee.Contract.MinConsecutiveShifts)
                    {
                        feasible = false;
                        _messages.Add($"{employee.Id} works {consecutiveShiftCount.Length} number of days in a row instead of the minimum required {employee.Contract.MinConsecutiveShifts}");
                    }
                }
            }

            return feasible;

            bool ConsecutiveShiftIsNotContinuationOfPreviousOrNextSchedulePeriod(ConsecutiveShiftLength consecutiveShiftLength)
            {
                if (consecutiveShiftLength.DayStart == 0) return false;
                if (consecutiveShiftLength.DayStart + consecutiveShiftLength.Length == _schedulingBenchmarkModel.Duration) return false;

                return true;
            }
        }

        public bool MaxConsecutiveShiftsNotExceeded(Employee employee, EmployeeEvaluationAggregate aggregate)
        {
            var feasible = true;

            foreach (var consecutiveShiftCount in aggregate.ConsecutiveShiftLengths)
            {
                if (consecutiveShiftCount.Length > employee.Contract.MaxConsecutiveShifts)
                {
                    feasible = false;
                    _messages.Add($"{employee.Id} works {consecutiveShiftCount.Length} number of days in a row instead of the maximum allowed {employee.Contract.MaxConsecutiveShifts}");
                }
            }

            return feasible;
        }

        public bool MinConsecutiveDaysOffNotExceeded(Employee employee, EmployeeEvaluationAggregate aggregate)
        {
            var feasible = true;

            foreach (var consecutiveDayOffCount in aggregate.DayOffLengths)
            {
                if (consecutiveDayOffCount < employee.Contract.MinConsecutiveDayOffs)
                {
                    feasible = false;
                    _messages.Add($"{employee.Id} has {consecutiveDayOffCount} consecutive days off instead of the minimum required {employee.Contract.MinConsecutiveDayOffs}");
                }
            }

            return feasible;
        }

        public bool MaxNumberOfWeekendsNotExceeded(Employee employee, EmployeeEvaluationAggregate aggregate)
        {
            var feasible = true;

            if (aggregate.WorkedWeekendsCount > employee.Contract.MaxWorkingWeekendCount)
            {
                feasible = false;
                _messages.Add($"{employee.Id} works on {aggregate.WorkedWeekendsCount} weekends instead of the maximum allowed {employee.Contract.MaxWorkingWeekendCount}");
            }

            return feasible;
        }

        public bool DayOffsRespected(Employee employee, EmployeeEvaluationAggregate aggregate)
        {
            var feasible = true;

            foreach (var dayOff in aggregate.DayOffsWithAssignments)
            {
                feasible = false;
                _messages.Add($"{employee.Id} has assignment on day {dayOff} instead of having a day off");
            }

            return feasible;
        }

        public bool MinRestTimeRespected(Employee employee, EmployeeEvaluationAggregate aggregate)
        {
            var feasible = true;

            foreach (var restTime in aggregate.RestTimes)
            {
                if (restTime < employee.Contract.MinRestTime)
                {
                    feasible = false;
                    _messages.Add($"{employee.Id} rests only {restTime} minutes"); 
                }
            }

            return feasible;
        }
    }
}
