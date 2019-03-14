using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class ConsecutiveShiftCountStateCalculator : IStateCalculator<int>
    {
        public int CalculateState(Person person, int day)
        {
            return person.Assignments.AllRounds.ContainsKey(day - 1)
                ? person.State.ConsecutiveShiftCount + 1
                : 0;
        }

        public int InitializeState(Person person) => 0;
    }
}
