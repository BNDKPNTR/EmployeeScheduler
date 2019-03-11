using System;
using System.Collections.Generic;

namespace SchedulingBenchmarks.Evaluators
{
    public class EmployeeFeasibilityAggregate
    {
        public string EmployeeId { get; }
        public Dictionary<string, int> ShiftCounts { get; }
        public int TotalWorkedMinutes { get; }
        public List<int> ConsecutiveShiftLengths { get; }
        public List<int> DayOffLengths { get; }
        public int WorkedWeekendsCount { get; }
        public List<int> DayOffsWithAssignments { get; }
        public List<int> RestTimes { get; }

        public EmployeeFeasibilityAggregate(string employeeId, Dictionary<string, int> shiftCounts, int totalWorkedMinutes, List<int> consecutiveShiftLengths, List<int> dayOffLengths, int workedWeekendsCount, List<int> dayOffsWithAssignments, List<int> restTimes)
        {
            EmployeeId = employeeId ?? throw new ArgumentNullException(nameof(employeeId));
            ShiftCounts = shiftCounts ?? throw new ArgumentNullException(nameof(shiftCounts));
            TotalWorkedMinutes = totalWorkedMinutes;
            ConsecutiveShiftLengths = consecutiveShiftLengths ?? throw new ArgumentNullException(nameof(consecutiveShiftLengths));
            DayOffLengths = dayOffLengths ?? throw new ArgumentNullException(nameof(dayOffLengths));
            WorkedWeekendsCount = workedWeekendsCount;
            DayOffsWithAssignments = dayOffsWithAssignments ?? throw new ArgumentNullException(nameof(dayOffsWithAssignments));
            RestTimes = restTimes ?? throw new ArgumentNullException(nameof(restTimes));
        }
    }
}
