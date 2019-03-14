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

        public ConsecutiveShiftCostFunction(Calendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
            _underMinConsecutiveShiftCount = DefaultCost / 100.0;
            _betweenMinAndMaxShiftCount = DefaultCost * 0.9;
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (!CanWorkMinConsecutiveShiftsInFuture(person, day)) return MaxCost;

            var consecutiveShiftCount = GetConsecutiveShiftCount(person, day);
            if (consecutiveShiftCount > person.WorkSchedule.MaxConsecutiveShifts) return MaxCost;

            // If not working consecutively
            if (person.State.ConsecutiveShiftCount == 0) return DefaultCost;

            // In case of the min consecutive shifts we assume that before the schedule period there was an infinite number of shifts
            // TODO: same assumption applies to the end of the schedule period
            var schedulePeriodStartFilter = person.WorkSchedule.MinConsecutiveShifts;
            if (day > schedulePeriodStartFilter && person.State.ConsecutiveShiftCount < person.WorkSchedule.MinConsecutiveShifts) return _underMinConsecutiveShiftCount;

            return _betweenMinAndMaxShiftCount;
        }

        private int GetConsecutiveShiftCount(Person person, int day)
        {
            var consecutiveShiftCount = person.State.ConsecutiveShiftCount;

            day++;
            while (person.Assignments.AllRounds.ContainsKey(day))
            {
                day++;
                consecutiveShiftCount++;
            }

            return consecutiveShiftCount + 1;
        }

        private bool CanWorkMinConsecutiveShiftsInFuture(Person person, int day)
        {
            if (person.State.ConsecutiveShiftCount == 0)
            {
                var length = Math.Min(day + person.WorkSchedule.MinConsecutiveShifts, person.Availabilities.Length);
                for (int i = day + 1; i < length; i++)
                {
                    if (!person.Availabilities[i])
                    {
                        return false;
                    }

                    if ((_calendar.IsSaturday(i) || _calendar.IsSunday(i)) && person.State.WorkedWeekendCount >= person.WorkSchedule.MaxWorkingWeekendCount)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
