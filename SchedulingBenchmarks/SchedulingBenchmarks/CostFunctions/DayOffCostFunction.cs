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
            if (!CanRestEnoughBetweenTodaysShiftAndNextShift(person, day)) return MaxCost;

            return 0 < person.State.DayOffCount && person.State.DayOffCount < person.WorkSchedule.MinConsecutiveDayOffs
                ? MaxCost
                : DefaultCost;
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
    }
}
