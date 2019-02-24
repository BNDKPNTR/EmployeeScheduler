using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class ShiftRequestCostFunction : CostFunctionBase
    {
        private readonly double _shiftOffRequestCost;
        private readonly double _shiftOnRequestCost;

        public ShiftRequestCostFunction()
        {
            _shiftOffRequestCost = DefaultCost * 2.0;
            _shiftOnRequestCost = DefaultCost / 5.0;
        }

        public override double CalculateCost(Person person, Demand demand, int timeSlot)
        {
            if (person.ShiftOffRequests[timeSlot]) return _shiftOffRequestCost;
            if (person.ShiftOnRequests[timeSlot]) return _shiftOnRequestCost;

            return DefaultCost;
        }
    }
}
