using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class TotalWorkTimeStateCalculator : StateCalculatorBase<int>
    {
        public override int CalculateState(Person person, State newState, StateTriggers triggers, int day)
        {
            if (person.Assignments.LatestRound.TryGetValue(day - 1, out var assignment))
            {
                return person.State.TotalWorkTime + assignment.Shift.Duration;
            }

            return person.State.TotalWorkTime;
        }

        public override int InitializeState(Person person, State newState) => person.Assignments.AllRounds.Values.Sum(a => a.Shift.Duration);
    }
}
