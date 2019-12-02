using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.Schedulers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.CostFunctions
{
    abstract class CostFunctionBase
    {
        private const double defaultCost = 1.0;

        public double MaxCost { get; }
        protected double DefaultCost => defaultCost;

        public CostFunctionBase()
        {
            MaxCost = SchedulerBase.UseJonkerVolgenant ? 1000 * 1000 : 1000;
        }

        public abstract double CalculateCost(Person person, Demand demand, int day);
    }
}
