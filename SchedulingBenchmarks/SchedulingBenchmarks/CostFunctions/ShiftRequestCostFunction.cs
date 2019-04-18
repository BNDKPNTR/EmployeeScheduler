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
            _shiftOffRequestCost = DefaultCost * 1.2;
            _shiftOnRequestCost = DefaultCost * 0.8;
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (person.ShiftRequests.TryGetValue(day, out var request) && request.ShiftId == demand.Shift.Id)
            {
                switch (request.Type)
                {
                    case RequestType.On: return _shiftOnRequestCost * request.Weight;
                    case RequestType.Off: return _shiftOffRequestCost * 1.0 / request.Weight;
                    default: throw new ArgumentOutOfRangeException(nameof(RequestType));
                }
            }

            return DefaultCost;
        }
    }
}
