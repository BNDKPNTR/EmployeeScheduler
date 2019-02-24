using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class ConsecutiveShiftCostFunction : CostFunctionBase
    {
        private readonly double _underMinConsecutiveShiftCount;

        public ConsecutiveShiftCostFunction()
        {
            _underMinConsecutiveShiftCount = DefaultCost / 9.0;
        }

        public override double CalculateCost(Person person, Demand demand, int timeSlot)
        {
            if (person.State.ConsecutiveShiftCount + 1 > person.WorkSchedule.MaxConsecutiveShifts) return MaxCost;
            if (person.State.ConsecutiveShiftCount < person.WorkSchedule.MinConsecutiveShifts) return _underMinConsecutiveShiftCount;

            return DefaultCost;
        }
    }
}
