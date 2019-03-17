using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class MinRestTimeCostFunction : CostFunctionBase
    {
        private const int OneDayInMinutes = 24 * 60;

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (CantRestEnoughAfterPreviousWork(person, demand, day)) return MaxCost;
            if (CantRestEnoughBeforeNextWork(person, demand, day)) return MaxCost;

            return DefaultCost;
        }

        private bool CantRestEnoughAfterPreviousWork(Person person, Demand demand, int day)
        {
            if (person.Assignments.AllRounds.TryGetValue(day - 1, out var assignment))
            {
                var lastShiftEnd = assignment.Shift.StartTime + assignment.Shift.Duration;
                var newShiftStart = OneDayInMinutes + demand.Shift.StartTime;

                if (newShiftStart - lastShiftEnd < person.WorkSchedule.MinRestTime)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CantRestEnoughBeforeNextWork(Person person, Demand demand, int day)
        {
            if (person.Assignments.AllRounds.TryGetValue(day + 1, out var assignment))
            {
                var newShiftEnd = demand.Shift.StartTime + demand.Shift.Duration;
                var nextShiftStart = OneDayInMinutes + assignment.Shift.StartTime;

                if (nextShiftStart - newShiftEnd < person.WorkSchedule.MinRestTime)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
