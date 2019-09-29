using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks
{
    internal class WorkEligibilityChecker
    {
        private readonly SchedulerModel _model;

        public WorkEligibilityChecker(SchedulerModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public bool CanWorkOnDay(Person person, int day)
        {
            if (!Available(person, day)) return false;
            if (AlreadyHasAssignmentOnDay(person, day)) return false;
            if (WouldWorkLessThanMinConsecutiveDays(person, day)) return false;
            if (WouldWorkMoreThanMaxConsecutiveDays(person, day)) return false;
            if (WouldRestLessThanMinConsecutiveDayOff(person, day)) return false;
            if (WouldWorkMoreThanMaxWeekends(person, day)) return false;

            return true;
        }

        private bool Available(Person person, int day)
        {
            return person.Availabilities[day];
        }

        private bool AlreadyHasAssignmentOnDay(Person person, int day)
        {
            return person.Assignments.AllRounds.ContainsKey(day);
        }

        private bool WouldWorkLessThanMinConsecutiveDays(Person person, int day)
        {
            if (person.State.ConsecutiveWorkDayCount > 0) return false;
            if (day == 0) return false; // We assume that the person worked infinite numbers of days before the schedule period

            // TODO: check MaxTotalWorkTime

            for (int i = day; i < day + person.WorkSchedule.MinConsecutiveWorkDays; i++)
            {
                if (i < person.Availabilities.Length && !person.Availabilities[i]) return true;
                if (_model.Calendar.IsWeekend(i) && person.State.WorkedWeekendCount >= person.WorkSchedule.MaxWorkingWeekendCount) return true;
            }

            return false;
        }

        private bool WouldWorkMoreThanMaxConsecutiveDays(Person person, int day)
        {
            var alreadyWorkedConsecutiveDays = person.State.ConsecutiveWorkDayCount;
            var todaysPossibleWork = 1;
            var consecutiveWorkDaysInFuture = 0;

            // Count until has assignment AND consecutive work days DO NOT exceed max. consecutive workdays
            var dayIndex = day + 1;
            while (person.Assignments.AllRounds.ContainsKey(dayIndex++) && ++consecutiveWorkDaysInFuture < person.WorkSchedule.MaxConsecutiveWorkDays) { }

            return alreadyWorkedConsecutiveDays + todaysPossibleWork + consecutiveWorkDaysInFuture > person.WorkSchedule.MaxConsecutiveWorkDays;
        }

        private bool WouldRestLessThanMinConsecutiveDayOff(Person person, int day)
        {
            if (person.State.ConsecutiveWorkDayCount > 0)
            {
                //var maxConsecutiveShiftDay = day - p.State.ConsecutiveWorkDayCount + p.WorkSchedule.MaxConsecutiveWorkDays;

                //for (int i = maxConsecutiveShiftDay; i < maxConsecutiveShiftDay + p.WorkSchedule.MinConsecutiveDayOffs; i++)
                //{
                //    if (p.Assignments.AllRounds.ContainsKey(i)) return true;
                //}

                return false;
            }
            else
            {
                if (person.State.ConsecutiveDayOffCount < person.WorkSchedule.MinConsecutiveDayOffs) return true;

                // If today's shift would be the continuation of tomorrow's shift
                if (person.Assignments.AllRounds.ContainsKey(day + 1)) return false;

                /* Today's shift would be the first in a row. Check if person could work min. consecutive days and then rest min. consecutive days before next work */
                var firstDayOfRestPeriod = day + person.WorkSchedule.MinConsecutiveWorkDays;
                var lastDayOfRestPeriod = firstDayOfRestPeriod + person.WorkSchedule.MinConsecutiveDayOffs;

                for (int i = firstDayOfRestPeriod; i < lastDayOfRestPeriod; i++)
                {
                    if (person.Assignments.AllRounds.ContainsKey(i)) return true;
                }

                return false;
            }
        }

        private bool WouldWorkMoreThanMaxWeekends(Person person, int day)
        {
            if (!_model.Calendar.IsWeekend(day)) return false;

            return person.State.WorkedWeekendCount >= person.WorkSchedule.MaxWorkingWeekendCount;
        }
    }
}
