﻿using System;
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
            _underMinWorkTimeCost = DefaultCost / 3.0;
        }

        public override double CalculateCost(Person person, Demand demand, int timeSlot)
        {
            if (person.State.TotalWorkTime + WorkSchedule.ShiftLengthInMinutes > person.WorkSchedule.MaxTotalWorkTime) return MaxCost;
            if (person.State.TotalWorkTime < person.WorkSchedule.MinTotalWorkTime) return _underMinWorkTimeCost;

            return DefaultCost;
        }
    }
}