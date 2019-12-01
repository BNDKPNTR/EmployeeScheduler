using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class ConsecutiveWorkDayCountStateCalculator : StateCalculatorBase<int>
    {
        public override int CalculateState(Person person, StateTriggers triggers, int day)
        {
            return person.Assignments.AllRounds.ContainsKey(day - 1)
                ? person.State.ConsecutiveWorkDayCount + 1
                : 0;
        }

        public override int InitializeState(Person person) => 0;
    }
}
