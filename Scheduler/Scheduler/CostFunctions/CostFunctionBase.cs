﻿using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.CostFunctions
{
    abstract class CostFunctionBase
    {
        private const double maxCost = 10_000;
        private const double defaultCost = 1.0;

        public double MaxCost => maxCost;
        protected double DefaultCost => defaultCost;

        public abstract double CalculateCost(Person person, Demand demand, int timeSlot);
    }
}