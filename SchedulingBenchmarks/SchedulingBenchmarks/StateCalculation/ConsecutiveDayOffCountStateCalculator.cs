using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class ConsecutiveDayOffCountStateCalculator : IStateCalculator<int>
    {
        public int CalculateState(Person person, StateTriggers triggers, int day)
        {
            return triggers.WorkedOnPreviousDay
                ? 0
                : person.State.ConsecutiveDayOffCount + 1;
        }

        public int InitializeState(Person person) => person.WorkSchedule.MinConsecutiveDayOffs - 1;
    }
}
