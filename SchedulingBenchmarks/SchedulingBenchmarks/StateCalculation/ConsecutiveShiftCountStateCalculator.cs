using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class ConsecutiveShiftCountStateCalculator : IStateCalculator<ConsecutiveShiftCountStateCalculator.Result>
    {
        public Result CalculateState(Person person, int day)
        {
            return person.Assignments.AllRounds.ContainsKey(day - 1)
                ? new Result(person.State.ConsecutiveShiftCount + 1)
                : new Result(0);
        }

        public Result InitializeState(Person person) => new Result(0);

        public class Result : IStateCalculatorResult
        {
            public int ConsecutiveShiftCount { get; }

            public Result(int consecutiveShiftCount)
            {
                ConsecutiveShiftCount = consecutiveShiftCount;
            }

            public void Apply(Person person)
            {
                person.State.ConsecutiveShiftCount = ConsecutiveShiftCount;
            }
        }
    }
}
