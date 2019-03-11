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
        private readonly Dictionary<string, EmployeeFeasibilityAggregate> _aggregates;
        private bool _feasible;
        private readonly ConcurrentBag<string> _messages;
        private readonly Dictionary<string, TimeSpan> _shiftLengths;

        public FeasibilityEvaluator(SchedulingBenchmarkModel schedulingBenchmarkModel, Dictionary<string, EmployeeFeasibilityAggregate> aggregates)
        {
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));
            _aggregates = aggregates ?? throw new ArgumentNullException(nameof(aggregates));
            _feasible = true;
            _messages = new ConcurrentBag<string>();
            _shiftLengths = _schedulingBenchmarkModel.Shifts.ToDictionary(s => s.Id, s => s.Duration);
        }

        public static (bool feasible, List<string> messages) Evaluate(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            var aggregates = FeasibilityDataAggregator.GetAggregate(schedulingBenchmarkModel).ToDictionary(a => a.EmployeeId);
            return new FeasibilityEvaluator(schedulingBenchmarkModel, aggregates).Feasible();
        }

        private (bool feasible, List<string> messages) Feasible()
        {
            Parallel.ForEach(_schedulingBenchmarkModel.Employees, (employee, state) =>
            {
                var aggregate = _aggregates[employee.Id];

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
                if (consecutiveShiftCount < employee.Contract.MinConsecutiveShifts)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} works {consecutiveShiftCount} number of days in a row instead of the minimum required {employee.Contract.MinConsecutiveShifts}");
                }

                if (consecutiveShiftCount > employee.Contract.MaxConsecutiveShifts)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} works {consecutiveShiftCount} number of days in a row instead of the maximum allowed {employee.Contract.MaxConsecutiveShifts}");
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
                _feasible = false;
                _messages.Add($"{employee.Id} rests only {restTime} minutes");
            }
        }
    }
}
