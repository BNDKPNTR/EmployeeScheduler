using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class ConsecutiveWorkDayCountStateCalculator : IStateCalculator<ConsecutiveWorkDayCountStateCalculator.Result>
    {
        public Result CalculateState(Person person, StateTriggers triggers, int day)
        {
            return person.Assignments.AllRounds.ContainsKey(day - 1)
                ? new Result(person.State.ConsecutiveWorkDayCount + 1)
                : new Result(0);
        }

        public Result InitializeState(Person person) => new Result(0);

        public class Result : IStateCalculatorResult
        {
            public int ConsecutiveWorkDayCount { get; }

            public Result(int consecutiveShiftCount)
            {
                ConsecutiveWorkDayCount = consecutiveShiftCount;
            }

            public void Apply(State state)
            {
                state.ConsecutiveWorkDayCount = ConsecutiveWorkDayCount;
            }
        }
    }
}
