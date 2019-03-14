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
            _shiftOnRequestCost = DefaultCost / 2.0;
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (person.ShiftOffRequests[day]) return _shiftOffRequestCost;
            if (person.ShiftOnRequests[day]) return _shiftOnRequestCost;

            return DefaultCost;
        }
    }
}
