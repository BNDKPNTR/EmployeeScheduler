using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class ConsecutiveShiftCostFunction : CostFunctionBase
    {
        private readonly Calendar _calendar;
        private readonly double _underMinConsecutiveShiftCount;
        private readonly double _betweenMinAndMaxShiftCount;
        private readonly double _workStartMultiplier;

        public ConsecutiveShiftCostFunction(Calendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
            _underMinConsecutiveShiftCount = DefaultCost / 100.0;
            _betweenMinAndMaxShiftCount = DefaultCost * 0.5;
            _workStartMultiplier = 2;
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (WouldWorkLessThanMinConsecutiveDays(person, day) || WouldWorkMoreThanMaxConsecutiveDays(person, day)) return MaxCost;

            // If not working consecutively
            if (person.State.ConsecutiveWorkDayCount == 0) return CalculatePossibleWorkStartMultiplier(person);

            // In case of the min consecutive shifts we assume that before the schedule period there was an infinite number of shifts
            // TODO: same assumption applies to the end of the schedule period
            if (day != 0 && person.State.ConsecutiveWorkDayCount < person.WorkSchedule.MinConsecutiveWorkDays) return _underMinConsecutiveShiftCount;

            return _betweenMinAndMaxShiftCount;
        }

        private double CalculatePossibleWorkStartMultiplier(Person person)
        {
            var ratio = person.State.PossibleFutureWorkDayCount / (double)person.WorkSchedule.MaxConsecutiveWorkDays;

            return DefaultCost + (1.0 - ratio) * _workStartMultiplier;
        }

        private bool WouldWorkLessThanMinConsecutiveDays(Person person, int day)
        {
            if (person.State.ConsecutiveWorkDayCount > 0) return false;
            if (day == 0) return false; // We assume that the person worked infinite numbers of days before the schedule period

            // TODO: check MaxTotalWorkTime

            for (int i = day; i < day + person.WorkSchedule.MinConsecutiveWorkDays; i++)
            {
                if (i < person.Availabilities.Length && !person.Availabilities[i]) return true;
                if (_calendar.IsWeekend(i) && person.State.WorkedWeekendCount >= person.WorkSchedule.MaxWorkingWeekendCount) return true;
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
    }
}
