using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class MaxShiftCostFunction : CostFunctionBase
    {
        public override double CalculateCost(Person person, Demand demand, int timeSlot)
        {
            if (!person.WorkSchedule.MaxShifts.TryGetValue(demand.ShifId, out var maxShiftCount)) return DefaultCost;

            person.State.ShiftWorkedCount.TryGetValue(demand.ShifId, out var shiftWorkedCount);

            return shiftWorkedCount + 1 > maxShiftCount ? MaxCost : DefaultCost;
        }
    }
}
