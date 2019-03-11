using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks
{
    public class FeasibilityEvaluator
    {
        private readonly bool _evaluateQuickly;
        private readonly SchedulingBenchmarkModel _schedulingBenchmarkModel;
        private bool _feasible;
        private readonly ConcurrentBag<string> _messages;
        private readonly Dictionary<string, TimeSpan> _shiftLengths;

        public FeasibilityEvaluator(SchedulingBenchmarkModel schedulingBenchmarkModel, bool evaluateQuickly)
        {
            _evaluateQuickly = evaluateQuickly;
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));
            _feasible = true;
            _messages = new ConcurrentBag<string>();
            _shiftLengths = _schedulingBenchmarkModel.Shifts.ToDictionary(s => s.Id, s => s.Duration);
        }

        public static (bool feasible, List<string> messages) Evaluate(SchedulingBenchmarkModel schedulingBenchmarkModel)
            => new FeasibilityEvaluator(schedulingBenchmarkModel, evaluateQuickly: false).Feasible();

        public static bool EvaluateQuickly(SchedulingBenchmarkModel schedulingBenchmarkModel)
            => new FeasibilityEvaluator(schedulingBenchmarkModel, evaluateQuickly: true).Feasible().feasible;

        private (bool feasible, List<string> messages) Feasible()
        {
            Parallel.ForEach(_schedulingBenchmarkModel.Employees, (employee, state) =>
            {
                if (_evaluateQuickly && !_feasible) state.Stop();

                MaxAllowedNumberOfShiftsNotExceeded(employee);
                MinAndMaxTotalMinutesNotExceeded(employee);
                MinAndMaxConsecutiveShiftsNotExceeded(employee);
                MinConsecutiveDaysOffNotExceeded(employee);
                MaxNumberOfWeekendsNotExceeded(employee);
                DayOffsRespected(employee);
            });

            return (_feasible, _messages.ToList());
        }

        private void MaxAllowedNumberOfShiftsNotExceeded(Employee employee)
        {
            var shiftCounts = employee.Assignments.Values.GroupBy(a => a.ShiftId).ToDictionary(a => a.Key, g => g.Count());

            foreach (var maxShift in employee.Contract.MaxShifts)
            {
                if (shiftCounts.TryGetValue(maxShift.Key, out var shiftCount) && shiftCount > maxShift.Value)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} has {shiftCount} assigned shifts of type {maxShift.Key} instead of the maximum allowed {maxShift.Value}");

                    if (_evaluateQuickly) return;
                }
            }
        }

        private void MinAndMaxTotalMinutesNotExceeded(Employee employee)
        {
            var totalWorkedMinutes = employee.Assignments.Values.Sum(a => _shiftLengths[a.ShiftId].TotalMinutes);

            if (totalWorkedMinutes < employee.Contract.MinTotalWorkTime)
            {
                _feasible = false;
                _messages.Add($"{employee.Id} works {totalWorkedMinutes} minutes instead of the minimum required {employee.Contract.MinTotalWorkTime}");
            }

            if (totalWorkedMinutes > employee.Contract.MaxTotalWorkTime)
            {
                _feasible = false;
                _messages.Add($"{employee.Id} works {totalWorkedMinutes} minutes instead of the maximum allowed {employee.Contract.MaxTotalWorkTime}");
            }
        }

        private void MinAndMaxConsecutiveShiftsNotExceeded(Employee employee)
        {
            foreach (var consecutiveShiftCount in GetConsecutiveShiftCounts(employee.Assignments))
            {
                if (consecutiveShiftCount < employee.Contract.MinConsecutiveShifts)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} works {consecutiveShiftCount} number of days in a row instead of the minimum required {employee.Contract.MinConsecutiveShifts}");

                    if (_evaluateQuickly) return;
                }

                if (consecutiveShiftCount > employee.Contract.MaxConsecutiveShifts)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} works {consecutiveShiftCount} number of days in a row instead of the maximum allowed {employee.Contract.MaxConsecutiveShifts}");

                    if (_evaluateQuickly) return;
                }
            }

            IEnumerable<int> GetConsecutiveShiftCounts(SortedDictionary<int, Assignment> assignments)
            {
                int consecutiveShiftCount;
                int lastDayWithAssignment;

                if (assignments.Keys.Count > 0)
                {
                    lastDayWithAssignment = assignments.Keys.First();
                    consecutiveShiftCount = 1;
                }
                else
                {
                    yield break;
                }

                foreach (var dayWithShift in assignments.Keys.Skip(1))
                {
                    if (lastDayWithAssignment + 1 == dayWithShift)
                    {
                        lastDayWithAssignment++;
                        consecutiveShiftCount++;
                    }
                    else
                    {
                        yield return consecutiveShiftCount;
                        lastDayWithAssignment = dayWithShift;
                        consecutiveShiftCount = 1;
                    }
                }

                yield return consecutiveShiftCount;
            }
        }

        private void MinConsecutiveDaysOffNotExceeded(Employee employee)
        {
            foreach (var consecutiveDayOffCount in GetConsecutiveDayOffCounts(employee.Assignments))
            {
                if (consecutiveDayOffCount < employee.Contract.MinConsecutiveDayOffs)
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} has {consecutiveDayOffCount} consecutive days off instead of the minimum required {employee.Contract.MinConsecutiveDayOffs}");

                    if (_evaluateQuickly) return;
                }
            }

            IEnumerable<int> GetConsecutiveDayOffCounts(SortedDictionary<int, Assignment> assignments)
            {
                int consecutiveDayOffCount = int.MaxValue / 2;

                if (employee.Assignments.ContainsKey(0))
                {
                    consecutiveDayOffCount = 0;
                }

                for (int i = 1; i < _schedulingBenchmarkModel.Duration; i++)
                {
                    if (employee.Assignments.ContainsKey(i))
                    {
                        if (consecutiveDayOffCount > 0)
                        {
                            yield return consecutiveDayOffCount;
                        }

                        consecutiveDayOffCount = 0;
                    }
                    else
                    {
                        consecutiveDayOffCount++;
                    }
                }

                // A tervezési időszak után végtelen pihenőnap van a specifikáció szerint, ezért az utolsó pihenő periódust nem kell visszaadnunk
            }
        }

        private void MaxNumberOfWeekendsNotExceeded(Employee employee)
        {
            var workedOnWeekendCount = 0;

            foreach (var (saturday, sunday) in Enumerable.Range(0, _schedulingBenchmarkModel.Duration / 7).Select(i => (i * 7 + 6, i * 7 + 7)))
            {
                if (employee.Assignments.ContainsKey(saturday) || employee.Assignments.ContainsKey(sunday))
                {
                    workedOnWeekendCount++;
                }
            }

            if (workedOnWeekendCount > employee.Contract.MaxWorkingWeekendCount)
            {
                _feasible = false;
                _messages.Add($"{employee.Id} works on {workedOnWeekendCount} weekends instead of the maximum allowed {employee.Contract.MaxWorkingWeekendCount}");
            }
        }

        private void DayOffsRespected(Employee employee)
        {
            foreach (var dayOff in employee.DayOffs)
            {
                if (employee.Assignments.ContainsKey(dayOff))
                {
                    _feasible = false;
                    _messages.Add($"{employee.Id} has assignment on day {dayOff} instead of having a day off");

                    if (_evaluateQuickly) return;
                }
            }
        }
    }
}
