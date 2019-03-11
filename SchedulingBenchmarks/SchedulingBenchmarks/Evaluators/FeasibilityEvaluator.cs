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
        private bool _feasible;
        private readonly ConcurrentBag<string> _messages;

        public FeasibilityEvaluator(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));
            _feasible = true;
            _messages = new ConcurrentBag<string>();
        }

        public static (bool feasible, List<string> messages) Evaluate(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            var aggregates = FeasibilityDataAggregator.GetAggregate(schedulingBenchmarkModel);
            return new FeasibilityEvaluator(schedulingBenchmarkModel).Feasible(aggregates);
        }

        private (bool feasible, List<string> messages) Feasible(IEnumerable<EmployeeFeasibilityAggregate> feasibilityAggregates)
        {
            var aggregates = feasibilityAggregates.ToDictionary(a => a.EmployeeId);

            Parallel.ForEach(_schedulingBenchmarkModel.Employees, employee =>
            {
                var aggregate = aggregates[employee.Id];

                MaxAllowedNumberOfShiftsNotExceeded(employee, aggregate);
                MinAndMaxTotalMinutesNotExceeded(employee, aggregate);
                MinAndMaxConsecutiveShiftsNotExceeded(employee, aggregate);
                MinConsecutiveDaysOffNotExceeded(employee, aggregate);
                MaxNumberOfWeekendsNotExceeded(employee, aggregate);
                DayOffsRespected(employee, aggregate);
                MinRestTimeRespected(employee, aggregate);
            });

            return (_feasible, _messages.ToList());
        }

        private void MaxAllowedNumberOfShiftsNotExceeded(Employee employee, EmployeeFeasibilityAggregate aggregate)
        {
            foreach (var maxShift in employee.Contract.MaxShifts)
            {
                if (aggregate.ShiftCounts.TryGetValue(maxShift.Key, out var shiftCount) && shiftCount > maxShift.Value)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} has {shiftCount} assigned shifts of type {maxShift.Key} instead of the maximum allowed {maxShift.Value}");
                }
            }
        }

        private void MinAndMaxTotalMinutesNotExceeded(Employee employee, EmployeeFeasibilityAggregate aggregate)
        {

            if (aggregate.TotalWorkedMinutes < employee.Contract.MinTotalWorkTime)
            {
                _feasible = false;
                _messages.Add($"{employee.Id} works {aggregate.TotalWorkedMinutes} minutes instead of the minimum required {employee.Contract.MinTotalWorkTime}");
            }

            if (aggregate.TotalWorkedMinutes > employee.Contract.MaxTotalWorkTime)
            {
                _feasible = false;
                _messages.Add($"{employee.Id} works {aggregate.TotalWorkedMinutes} minutes instead of the maximum allowed {employee.Contract.MaxTotalWorkTime}");
            }
        }

        private void MinAndMaxConsecutiveShiftsNotExceeded(Employee employee, EmployeeFeasibilityAggregate aggregate)
        {
            foreach (var consecutiveShiftCount in aggregate.ConsecutiveShiftLengths)
            {
                // In case of the min consecutive shifts we assume that before the schedule period there was an infinite number of shifts
                var schedulePeriodEndFilter = _schedulingBenchmarkModel.Duration - employee.Contract.MinConsecutiveShifts + 1;
                var remainingDaysFromSchedulePeriod = _schedulingBenchmarkModel.Duration - 1 - consecutiveShiftCount.DayStart;
                if (0 < consecutiveShiftCount.DayStart 
                    && (consecutiveShiftCount.DayStart < schedulePeriodEndFilter && consecutiveShiftCount.Length == remainingDaysFromSchedulePeriod))
                {
                    if (consecutiveShiftCount.Length < employee.Contract.MinConsecutiveShifts)
                    {
                        _feasible = false;
                        _messages.Add($"{employee.Id} works {consecutiveShiftCount.Length} number of days in a row instead of the minimum required {employee.Contract.MinConsecutiveShifts}");
                    }
                }

                if (consecutiveShiftCount.Length > employee.Contract.MaxConsecutiveShifts)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} works {consecutiveShiftCount.Length} number of days in a row instead of the maximum allowed {employee.Contract.MaxConsecutiveShifts}");
                }
            }
        }

        private void MinConsecutiveDaysOffNotExceeded(Employee employee, EmployeeFeasibilityAggregate aggregate)
        {
            foreach (var consecutiveDayOffCount in aggregate.DayOffLengths)
            {
                if (consecutiveDayOffCount < employee.Contract.MinConsecutiveDayOffs)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} has {consecutiveDayOffCount} consecutive days off instead of the minimum required {employee.Contract.MinConsecutiveDayOffs}");
                }
            }
        }

        private void MaxNumberOfWeekendsNotExceeded(Employee employee, EmployeeFeasibilityAggregate aggregate)
        {
            if (aggregate.WorkedWeekendsCount > employee.Contract.MaxWorkingWeekendCount)
            {
                _feasible = false;
                _messages.Add($"{employee.Id} works on {aggregate.WorkedWeekendsCount} weekends instead of the maximum allowed {employee.Contract.MaxWorkingWeekendCount}");
            }
        }

        private void DayOffsRespected(Employee employee, EmployeeFeasibilityAggregate aggregate)
        {
            foreach (var dayOff in aggregate.DayOffsWithAssignments)
            {
                _feasible = false;
                _messages.Add($"{employee.Id} has assignment on day {dayOff} instead of having a day off");
            }
        }

        private void MinRestTimeRespected(Employee employee, EmployeeFeasibilityAggregate aggregate)
        {
            foreach (var restTime in aggregate.RestTimes)
            {
                if (restTime < employee.Contract.MinRestTime)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} rests only {restTime} minutes"); 
                }
            }
        }
    }
}
