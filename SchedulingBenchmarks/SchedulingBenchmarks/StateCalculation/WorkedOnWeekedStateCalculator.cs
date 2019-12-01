using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class WorkedOnWeekedStateCalculator : StateCalculatorBase<bool>
    {
        private readonly Calendar _calendar;

        public WorkedOnWeekedStateCalculator(Calendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

        public override bool CalculateState(Person person, State newState, StateTriggers triggers, int day)
        {
            if (person.State.WorkedOnWeeked) return true;

            return _calendar.IsSunday(day) || _calendar.IsMonday(day)
                ? person.Assignments.LatestRound.ContainsKey(day - 1)
                : person.State.WorkedOnWeeked;
        }

        public override bool InitializeState(Person person, State newState) => false;
    }
}
