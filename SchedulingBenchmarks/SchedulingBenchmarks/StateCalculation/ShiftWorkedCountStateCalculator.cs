using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class ShiftWorkedCountStateCalculator : StateCalculatorBase<Dictionary<Shift, int>>
    {
        public override Dictionary<Shift, int> CalculateState(Person person, StateTriggers triggers, int day)
        {
            if (!person.Assignments.LatestRound.TryGetValue(day - 1, out var previousAssignment)) return person.State.ShiftWorkedCount;

            person.State.ShiftWorkedCount.TryGetValue(previousAssignment.Shift, out var shiftWorkedCount);
            person.State.ShiftWorkedCount[previousAssignment.Shift] = shiftWorkedCount + 1;

            return person.State.ShiftWorkedCount;
        }

        public override Dictionary<Shift, int> InitializeState(Person person) 
            => person.Assignments.AllRounds.Values.GroupBy(a => a.Shift).ToDictionary(g => g.Key, g => g.Count());
    }
}
