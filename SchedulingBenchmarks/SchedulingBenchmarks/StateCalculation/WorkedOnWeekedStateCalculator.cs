using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class WorkedOnWeekedStateCalculator : IStateCalculator<WorkedOnWeekedStateCalculator.Result>
    {
        private readonly Calendar _calendar;

        public WorkedOnWeekedStateCalculator(Calendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

        public Result CalculateState(Person person, StateTriggers triggers, int day)
        {
            if (person.State.WorkedOnWeeked) return new Result(true);

            return _calendar.IsSunday(day) || _calendar.IsMonday(day)
                ? new Result(person.Assignments.LatestRound.ContainsKey(day - 1))
                : new Result(person.State.WorkedOnWeeked);
        }

        public Result InitializeState(Person person) => new Result(false);

        public class Result : IStateCalculatorResult
        {
            public bool WorkedOnWeeked { get; }

            public Result(bool workedOnWeeked)
            {
                WorkedOnWeeked = workedOnWeeked;
            }

            public void Apply(State state)
            {
                state.WorkedOnWeeked = WorkedOnWeeked;
            }
        }
    }
}
