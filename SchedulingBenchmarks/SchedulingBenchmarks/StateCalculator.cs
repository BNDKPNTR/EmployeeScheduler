using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    class StateCalculator
    {
        public void RefreshState(Person person, int timeSlot)
        {
            person.State.WorkedOnWeeked = CalculateWorkedOnWeekend(person, timeSlot);
            person.State.TotalWorkTime = CalculateTotalWorkTime(person, timeSlot);
            person.State.ConsecutiveShiftCount = CalculateConsecutiveShiftCount(person, timeSlot);
            person.State.DayOffCount = CalculateDayOffCount(person, timeSlot);
            person.State.WorkedWeekendCount = CalculateWorkedWeekendCount(person, timeSlot);
            person.State.ShiftWorkedCount = CalculateShiftWorkedCount(person, timeSlot);
        }

        private Dictionary<Shift, int> CalculateShiftWorkedCount(Person person, int timeSlot)
        {
            if (!person.Assignments.TryGetValue(timeSlot - 1, out var previousAssignment)) return person.State.ShiftWorkedCount;

            person.State.ShiftWorkedCount.TryGetValue(previousAssignment.Shift, out var shiftWorkedCount);
            person.State.ShiftWorkedCount[previousAssignment.Shift] = shiftWorkedCount + 1;

            return person.State.ShiftWorkedCount;
        }

        private int CalculateWorkedWeekendCount(Person person, int timeSlot)
        {
            if (!IsMonday(timeSlot)) return person.State.WorkedWeekendCount;

            return person.Assignments.ContainsKey(timeSlot - 2) || person.Assignments.ContainsKey(timeSlot - 1)
                ? person.State.WorkedWeekendCount + 1
                : person.State.WorkedWeekendCount;
        }

        private int CalculateDayOffCount(Person person, int timeSlot)
        {
            return person.Assignments.ContainsKey(timeSlot - 1)
                ? 0
                : person.State.DayOffCount + 1;
        }

        private int CalculateConsecutiveShiftCount(Person person, int timeSlot)
        {
            return person.Assignments.ContainsKey(timeSlot - 1)
                ? person.State.ConsecutiveShiftCount + 1
                : 0;
        }

        private int CalculateTotalWorkTime(Person person, int timeSlot)
        {
            if (person.Assignments.TryGetValue(timeSlot - 1, out var assignment))
            {
                return person.State.TotalWorkTime + assignment.Shift.Duration;
            }

            return person.State.TotalWorkTime;
        }

        private bool CalculateWorkedOnWeekend(Person person, int timeSlot)
        {
            if (person.State.WorkedOnWeeked) return true;

            return IsSundayOrMonday(timeSlot)
                ? person.Assignments.ContainsKey(timeSlot - 1)
                : person.State.WorkedOnWeeked;
        }

        private bool IsMonday(int timeSlot)
        {
            var dayOfWeek = timeSlot % 7;

            return dayOfWeek == 0;
        }

        private bool IsSundayOrMonday(int timeSlot)
        {
            var dayOfWeek = timeSlot % 7;

            return dayOfWeek == 6 || dayOfWeek == 0;
        }
    }
}
