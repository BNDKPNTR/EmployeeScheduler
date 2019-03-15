using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class ConsecutiveDayOffCountStateCalculator : IStateCalculator<ConsecutiveDayOffCountStateCalculator.Result>
    {
        public Result CalculateState(Person person, StateTriggers triggers, int day)
        {
            return triggers.WorkedOnPreviousDay
                ? new Result(0)
                : new Result(person.State.ConsecutiveDayOffCount + 1);
        }

        public Result InitializeState(Person person) => new Result(person.WorkSchedule.MinConsecutiveDayOffs - 1);

        public class Result : IStateCalculatorResult
        {
            public int ConsecutiveDayOffCount { get; }

            public Result(int dayOffCount)
            {
                ConsecutiveDayOffCount = dayOffCount;
            }

            public void Apply(State state)
            {
                state.ConsecutiveDayOffCount = ConsecutiveDayOffCount;
            }
        }
    }
}
