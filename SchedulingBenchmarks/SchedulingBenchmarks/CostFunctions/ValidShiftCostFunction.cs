using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class ValidShiftCostFunction : CostFunctionBase
    {
        public override double CalculateCost(Person person, Demand demand, int timeSlot)
        {
            return person.WorkSchedule.ValidShifts.Contains(demand.ShifId) ? DefaultCost : MaxCost;
        }
    }
}
