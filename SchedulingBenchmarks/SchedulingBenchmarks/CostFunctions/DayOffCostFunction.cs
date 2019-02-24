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
            return 0 < person.State.DayOffCount && person.State.DayOffCount < person.WorkSchedule.MinConsecutiveDayOffs 
                ? MaxCost 
                : DefaultCost;
        }
    }
}
