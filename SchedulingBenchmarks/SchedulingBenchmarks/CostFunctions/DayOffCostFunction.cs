using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class DayOffCostFunction : CostFunctionBase
    {
        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (WouldRestLessThanMinConsecutiveDayOff(person, day)) return MaxCost;

            if (!CanRestEnoughBetweenTodaysShiftAndNextShift(person, day)) return MaxCost;

            return DefaultCost;
        }

        private bool CanRestEnoughBetweenTodaysShiftAndNextShift(Person person, int day)
        {
            // If today's shift will be the continuation of tomorrow's shift
            if (person.Assignments.AllRounds.ContainsKey(day + 1)) return true;

            // Otherwise check if there's no work for at least X days
            for (int i = 1; i <= person.WorkSchedule.MinConsecutiveDayOffs; i++)
            {
                if (person.Assignments.AllRounds.ContainsKey(day + i))
                {
                    return false;
                }
            }

            return true;
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
    }
}
