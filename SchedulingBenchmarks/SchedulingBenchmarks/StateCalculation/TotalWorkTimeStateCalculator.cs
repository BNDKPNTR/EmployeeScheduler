using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class TotalWorkTimeStateCalculator : IStateCalculator<TotalWorkTimeStateCalculator.Result>
    {
        public Result CalculateState(Person person, StateTriggers triggers, int day)
        {
            if (person.Assignments.LatestRound.TryGetValue(day - 1, out var assignment))
            {
                return new Result(person.State.TotalWorkTime + assignment.Shift.Duration);
            }

            return new Result(person.State.TotalWorkTime);
        }

        public Result InitializeState(Person person) => new Result(person.Assignments.AllRounds.Values.Sum(a => a.Shift.Duration));

        public class Result : IStateCalculatorResult
        {
            public int TotalWorkTime { get; }

            public Result(int totalWorkTime)
            {
                TotalWorkTime = totalWorkTime;
            }

            public void Apply(State state)
            {
                state.TotalWorkTime = TotalWorkTime;
            }
        }
    }
}
