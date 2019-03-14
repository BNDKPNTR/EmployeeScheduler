using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class AvailabilityCostFunction : CostFunctionBase
    {
        public override double CalculateCost(Person person, Demand demand, int day) 
            => person.Availabilities[day] ? DefaultCost : MaxCost;
    }
}
