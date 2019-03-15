using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class ShiftWorkedCountStateCalculator : IStateCalculator<ShiftWorkedCountStateCalculator.Result>
    {
        public Result CalculateState(Person person, StateTriggers triggers, int day)
        {
            if (!person.Assignments.LatestRound.TryGetValue(day - 1, out var previousAssignment)) return new Result(person.State.ShiftWorkedCount);

            person.State.ShiftWorkedCount.TryGetValue(previousAssignment.Shift, out var shiftWorkedCount);
            person.State.ShiftWorkedCount[previousAssignment.Shift] = shiftWorkedCount + 1;

            return new Result(person.State.ShiftWorkedCount);
        }

        public Result InitializeState(Person person) 
            => new Result(person.Assignments.AllRounds.Values.GroupBy(a => a.Shift).ToDictionary(g => g.Key, g => g.Count()));

        public class Result : IStateCalculatorResult
        {
            public Dictionary<Shift, int> ShiftWorkedCount { get; }

            public Result(Dictionary<Shift, int> shiftWorkedCount)
            {
                ShiftWorkedCount = shiftWorkedCount;
            }

            public void Apply(State state)
            {
                state.ShiftWorkedCount = ShiftWorkedCount;
            }
        }
    }
}
