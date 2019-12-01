using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class ConsecutiveDayOffCountStateCalculator : StateCalculatorBase<int>
    {
        public override int CalculateState(Person person, State newState, StateTriggers triggers, int day)
        {
            return triggers.WorkedOnPreviousDay
                ? 0
                : person.State.ConsecutiveDayOffCount + 1;
        }

        public override int InitializeState(Person person, State newState) => person.WorkSchedule.MinConsecutiveDayOffs - 1;
    }
}
