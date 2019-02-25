﻿using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    class StateCalculator
    {
        private readonly Calendar _calendar;

        public StateCalculator(Calendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

        public void RefreshState(Person person, int timeSlot)
        {
            person.State.WorkedOnWeeked = CalculateWorkedOnWeekend(person, timeSlot);
            person.State.TotalWorkTime = CalculateTotalWorkTime(person, timeSlot);
            person.State.ConsecutiveShiftCount = CalculateConsecutiveShiftCount(person, timeSlot);
            person.State.DayOffCount = CalculateDayOffCount(person, timeSlot);
            person.State.WorkedWeekendCount = CalculateWorkedWeekendCount(person, timeSlot);
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
            if (person.Assignments.ContainsKey(timeSlot - 1))
            {
                return person.State.TotalWorkTime + WorkSchedule.ShiftLengthInMinutes;
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
