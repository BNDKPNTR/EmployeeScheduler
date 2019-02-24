using System;
using System.Collections.Generic;
using System.Text;
using Scheduler.Models;

namespace Scheduler.CostFunctions
{
    class AvailabilityCostFunction : CostFunctionBase
    {
        public override double CalculateCost(Person person, Demand demand, int timeSlot) => person.Available ? DefaultCost : MaxCost;
    }
}
