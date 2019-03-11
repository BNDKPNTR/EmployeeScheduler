using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    class StateCalculator
    {
        private readonly Range _schedulePeriod;

        public StateCalculator(Range schedulePeriod)
        {
            _schedulePeriod = schedulePeriod;
        }

        public void RefreshState(Person person, int day)
        {
            person.State.WorkedOnWeeked = CalculateWorkedOnWeekend(person, day);
            person.State.TotalWorkTime = CalculateTotalWorkTime(person, day);
            person.State.ConsecutiveShiftCount = CalculateConsecutiveShiftCount(person, day);
            person.State.DayOffCount = CalculateDayOffCount(person, day);
            person.State.WorkedWeekendCount = CalculateWorkedWeekendCount(person, day);
            person.State.ShiftWorkedCount = CalculateShiftWorkedCount(person, day);
        }

        public void InitializeState(Person person)
        {
            person.State.WorkedOnWeeked = false;
            person.State.TotalWorkTime = person.Assignments.AllRounds.Values.Sum(a => a.Shift.Duration);
            person.State.ConsecutiveShiftCount = 0;
            person.State.DayOffCount = person.WorkSchedule.MinConsecutiveDayOffs - 1;
            person.State.WorkedWeekendCount = CalculateInitialWorkedWeekendCount(person);
            person.State.ShiftWorkedCount = person.Assignments.AllRounds.Values.GroupBy(a => a.Shift).ToDictionary(g => g.Key, g => g.Count());
        }

        private int CalculateInitialWorkedWeekendCount(Person person)
        {
            var workedOnWeekendCount = 0;

            foreach (var (assignmentOnSaturday, assignmentOnSunday) in GetWeekendAssignments(person.Assignments))
            {
                if (assignmentOnSaturday != null || assignmentOnSunday != null)
                {
                    workedOnWeekendCount++;
                }
            }

            return workedOnWeekendCount;

            IEnumerable<(Assignment assignmentOnSaturday, Assignment assignmentOnSunday)> GetWeekendAssignments(AssignmentsCollection assignments)
            {
                for (int i = _schedulePeriod.Start + 6; i < _schedulePeriod.Length; i += 7)
                {
                    assignments.AllRounds.TryGetValue(i, out var assignmentOnSaturday);
                    assignments.AllRounds.TryGetValue(i + 1, out var assignmentOnSunday);

                    yield return (assignmentOnSaturday, assignmentOnSunday);
                }
            }
        }

        private Dictionary<Shift, int> CalculateShiftWorkedCount(Person person, int day)
        {
            if (!person.Assignments.LatestRound.TryGetValue(day - 1, out var previousAssignment)) return person.State.ShiftWorkedCount;

            person.State.ShiftWorkedCount.TryGetValue(previousAssignment.Shift, out var shiftWorkedCount);
            person.State.ShiftWorkedCount[previousAssignment.Shift] = shiftWorkedCount + 1;

            return person.State.ShiftWorkedCount;
        }

        private int CalculateWorkedWeekendCount(Person person, int day)
        {
            if (!IsMonday(day)) return person.State.WorkedWeekendCount;

            return person.Assignments.LatestRound.ContainsKey(day - 2) || person.Assignments.LatestRound.ContainsKey(day - 1)
                ? person.State.WorkedWeekendCount + 1
                : person.State.WorkedWeekendCount;
        }

        private int CalculateDayOffCount(Person person, int day)
        {
            return person.Assignments.AllRounds.ContainsKey(day - 1)
                ? 0
                : person.State.DayOffCount + 1;
        }

        private int CalculateConsecutiveShiftCount(Person person, int day)
        {
            return person.Assignments.AllRounds.ContainsKey(day - 1)
                ? person.State.ConsecutiveShiftCount + 1
                : 0;
        }

        private int CalculateTotalWorkTime(Person person, int day)
        {
            if (person.Assignments.LatestRound.TryGetValue(day - 1, out var assignment))
            {
                return person.State.TotalWorkTime + assignment.Shift.Duration;
            }

            return person.State.TotalWorkTime;
        }

        private bool CalculateWorkedOnWeekend(Person person, int day)
        {
            if (person.State.WorkedOnWeeked) return true;

            return IsSundayOrMonday(day)
                ? person.Assignments.LatestRound.ContainsKey(day - 1)
                : person.State.WorkedOnWeeked;
        }

        private bool IsMonday(int day)
        {
            var dayOfWeek = day % 7;

            return dayOfWeek == 0;
        }

        private bool IsSundayOrMonday(int day)
        {
            var dayOfWeek = day % 7;

            return dayOfWeek == 6 || dayOfWeek == 0;
        }
    }
}
