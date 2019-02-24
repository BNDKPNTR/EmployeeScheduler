using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class WeekendWorkCostFunction : CostFunctionBase
    {
        public override double CalculateCost(Person person, Demand demand, int timeSlot)
        {
            if (!IsWeekend(timeSlot)) return DefaultCost;

            return person.State.WorkedOnWeeked ? MaxCost : DefaultCost;
        }

        private bool IsWeekend(int timeSlot)
        {
            var dayOfWeek = timeSlot % 7;

            return dayOfWeek == 5 || dayOfWeek == 6;
        }
    }
}
