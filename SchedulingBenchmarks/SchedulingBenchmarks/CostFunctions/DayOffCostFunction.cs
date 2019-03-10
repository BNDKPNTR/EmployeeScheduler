using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class DayOffCostFunction : CostFunctionBase
    {
        public override double CalculateCost(Person person, Demand demand, int timeSlot)
        {
            // While in work
            if (person.State.DayOffCount == 0)
            {
                return DefaultCost;
            }

            if (person.State.DayOffCount < person.WorkSchedule.MinConsecutiveDayOffs)
            {
                return MaxCost;
            }

            for (int i = 1; i <= person.WorkSchedule.MinConsecutiveDayOffs; i++)
            {
                if (person.Assignments.ContainsKey(timeSlot + i))
                {
                    return MaxCost;
                }
            }

            return DefaultCost;
        }
    }
}
