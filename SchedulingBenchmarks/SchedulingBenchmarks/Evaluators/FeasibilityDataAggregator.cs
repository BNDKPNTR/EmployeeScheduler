using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Evaluators
{
    public class FeasibilityDataAggregator
    {
        private readonly SchedulingBenchmarkModel _schedulingBenchmarkModel;
        private readonly Dictionary<string, TimeSpan> _shiftLengths;
        private readonly Dictionary<string, TimeSpan> _shiftStartTimes;

        public FeasibilityDataAggregator(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));
            _shiftLengths = _schedulingBenchmarkModel.Shifts.ToDictionary(s => s.Id, s => s.Duration);
            _shiftStartTimes = _schedulingBenchmarkModel.Shifts.ToDictionary(s => s.Id, s => s.StartTime);
        }

        public static List<EmployeeFeasibilityAggregate> GetAggregate(SchedulingBenchmarkModel schedulingBenchmarkModel)
            => new FeasibilityDataAggregator(schedulingBenchmarkModel).Feasible();

        private List<EmployeeFeasibilityAggregate> Feasible()
        {
            var aggregates = new ConcurrentBag<EmployeeFeasibilityAggregate>();

            Parallel.ForEach(_schedulingBenchmarkModel.Employees, employee =>
            {
                var shiftCounts = GetShiftCounts(employee);
                var totalWorkedMinutes = GetTotalWorkedMinutes(employee);
                var consecutiveShiftLengths = GetConsecutiveShiftLengths(employee);
                var dayOffLengths = GetDayOffLengths(employee);
                var workedWeekendsCount = GetWorkedWeekendsCount(employee);
                var dayOffsWithAssignments = GetDayOffsWithAssignments(employee);
                var restTimes = GetRestTimes(employee);

                var aggregate = new EmployeeFeasibilityAggregate(
                    employee.Id,
                    shiftCounts, 
                    totalWorkedMinutes, 
                    consecutiveShiftLengths, 
                    dayOffLengths, 
                    workedWeekendsCount, 
                    dayOffsWithAssignments, 
                    restTimes);

                aggregates.Add(aggregate);
            });

            return aggregates.ToList();
        }

        private Dictionary<string, int> GetShiftCounts(Employee employee)
        {
            return employee.Assignments.Values
                .GroupBy(a => a.ShiftId)
                .ToDictionary(a => a.Key, g => g.Count());
        }

        private int GetTotalWorkedMinutes(Employee employee)
        {
            return employee.Assignments.Values.Sum(a => (int)_shiftLengths[a.ShiftId].TotalMinutes);
        }

        private List<int> GetConsecutiveShiftLengths(Employee employee)
        {
            var consecutiveShiftLengths = new List<int>();

            int consecutiveShiftCount;
            int lastDayWithAssignment;

            if (employee.Assignments.Keys.Count > 0)
            {
                lastDayWithAssignment = employee.Assignments.Keys.First();
                consecutiveShiftCount = 1;
            }
            else
            {
                return consecutiveShiftLengths;
            }

            foreach (var dayWithShift in employee.Assignments.Keys.Skip(1))
            {
                if (lastDayWithAssignment + 1 == dayWithShift)
                {
                    lastDayWithAssignment++;
                    consecutiveShiftCount++;
                }
                else
                {
                    consecutiveShiftLengths.Add(consecutiveShiftCount);
                    lastDayWithAssignment = dayWithShift;
                    consecutiveShiftCount = 1;
                }
            }

            consecutiveShiftLengths.Add(consecutiveShiftCount);

            return consecutiveShiftLengths;
        }

        private List<int> GetDayOffLengths(Employee employee)
        {
            var dayOffLengths = new List<int>();
            int consecutiveDayOffCount = employee.Contract.MinConsecutiveDayOffs;

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
                        dayOffLengths.Add(consecutiveDayOffCount);
                    }

                    consecutiveDayOffCount = 0;
                }
                else
                {
                    consecutiveDayOffCount++;
                }
            }

            // A tervezési időszak után végtelen pihenőnap van a specifikáció szerint, ezért az utolsó pihenő periódust nem kell visszaadnunk

            return dayOffLengths;
        }

        private int GetWorkedWeekendsCount(Employee employee)
        {
            var workedOnWeekendCount = 0;

            foreach (var (saturday, sunday) in Enumerable.Range(0, _schedulingBenchmarkModel.Duration / 7).Select(i => (i * 7 + 5, i * 7 + 6)))
            {
                if (employee.Assignments.ContainsKey(saturday) || employee.Assignments.ContainsKey(sunday))
                {
                    workedOnWeekendCount++;
                }
            }

            return workedOnWeekendCount;
        }

        private List<int> GetDayOffsWithAssignments(Employee employee)
        {
            return employee.DayOffs.Where(dayOff => employee.Assignments.ContainsKey(dayOff)).ToList();
        }

        private List<int> GetRestTimes(Employee employee)
        {
            const int OneDayInMinutes = 24 * 60;

            return employee.Assignments.Values.Zip(employee.Assignments.Values.Skip(1), (x, y) =>
            {
                var firstAssignmentEndTimeInMinutes = 
                    OneDayInMinutes * x.Day 
                    + (int)_shiftStartTimes[x.ShiftId].TotalMinutes 
                    + (int)_shiftLengths[x.ShiftId].TotalMinutes;

                var secondAssignmentStartTimeInMinutes = OneDayInMinutes * y.Day + (int)_shiftStartTimes[y.ShiftId].TotalMinutes;

                return secondAssignmentStartTimeInMinutes - firstAssignmentEndTimeInMinutes;
            }).ToList();
        }
    }
}
