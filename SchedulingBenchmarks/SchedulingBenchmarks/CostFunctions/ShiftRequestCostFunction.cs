using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class ShiftRequestCostFunction : CostFunctionBase
    {
        private readonly double _oneUnitShiftOffRequestCost;
        private readonly double _oneUnitShiftOnRequestCost;

        public ShiftRequestCostFunction(int maxShiftOffRequestWeight, int maxShiftOnRequestWeight)
        {
            _oneUnitShiftOffRequestCost = (DefaultCost * 1.2 - DefaultCost) / maxShiftOffRequestWeight;
            _oneUnitShiftOnRequestCost = (DefaultCost - DefaultCost * 0.8) / maxShiftOnRequestWeight;
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (person.ShiftRequests.TryGetValue(day, out var request) && request.ShiftId == demand.Shift.Id)
            {
                switch (request.Type)
                {
                    case RequestType.On: return DefaultCost - _oneUnitShiftOnRequestCost * request.Weight;
                    case RequestType.Off: return DefaultCost + _oneUnitShiftOffRequestCost * request.Weight;
                    default: throw new ArgumentOutOfRangeException(nameof(RequestType));
                }
            }

            return DefaultCost;
        }

        public static (int maxShiftOffRequestWeight, int maxShiftOnRequestWeight) GetMaxWeights(SchedulerModel schedulerModel)
        {
            var maxShiftOffRequestWeight = schedulerModel.People
                .SelectMany(p => p.ShiftRequests)
                .Where(sr => sr.Value.Type == RequestType.Off)
                .Max(sr => sr.Value.Weight);

            var maxShiftOnRequestWeight = schedulerModel.People
                .SelectMany(p => p.ShiftRequests)
                .Where(sr => sr.Value.Type == RequestType.On)
                .Max(sr => sr.Value.Weight);

            return (maxShiftOffRequestWeight, maxShiftOnRequestWeight);
        }
    }
}
