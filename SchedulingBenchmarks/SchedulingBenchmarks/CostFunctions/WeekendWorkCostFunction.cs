using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class WeekendWorkCostFunction : CostFunctionBase
    {
        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (!IsWeekend(day)) return DefaultCost;

            return person.State.WorkedWeekendCount >= person.WorkSchedule.MaxWorkingWeekendCount ? MaxCost : DefaultCost;
        }

        private bool IsWeekend(int timeSlot)
        {
            var dayOfWeek = timeSlot % 7;

            return dayOfWeek == 5 || dayOfWeek == 6;
        }
    }
}
