using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.CostFunctions
{
    abstract class CostFunctionBase
    {
        private const double maxCost = 1_000;
        private const double defaultCost = 1.0;

        public double MaxCost => maxCost;
        protected double DefaultCost => defaultCost;

        public abstract double CalculateCost(Person person, Demand demand, int day);
    }
}
