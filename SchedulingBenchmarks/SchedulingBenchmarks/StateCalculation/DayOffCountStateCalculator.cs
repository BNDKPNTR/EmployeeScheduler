using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class DayOffCountStateCalculator : IStateCalculator<DayOffCountStateCalculator.Result>
    {
        public Result CalculateState(Person person, int day)
        {
            return person.Assignments.AllRounds.ContainsKey(day - 1)
                ? new Result(0)
                : new Result(person.State.DayOffCount + 1);
        }

        public Result InitializeState(Person person) => new Result(person.WorkSchedule.MinConsecutiveDayOffs - 1);

        public class Result : IStateCalculatorResult
        {
            public int DayOffCount { get; }

            public Result(int dayOffCount)
            {
                DayOffCount = dayOffCount;
            }

            public void Apply(Person person)
            {
                person.State.DayOffCount = DayOffCount;
            }
        }
    }
}
