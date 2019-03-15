using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class TotalWorkTimeCostFunction : CostFunctionBase
    {
        private readonly double _underMinWorkTimeCost;

        public TotalWorkTimeCostFunction()
        {
            _underMinWorkTimeCost = DefaultCost * 0.5;
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (person.State.TotalWorkTime + demand.Shift.Duration > person.WorkSchedule.MaxTotalWorkTime) return MaxCost;
            if (person.State.TotalWorkTime < person.WorkSchedule.MinTotalWorkTime) return _underMinWorkTimeCost;

            return DefaultCost;
        }
    }
}
