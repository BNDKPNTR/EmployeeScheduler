using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class DayOffCountStateCalculator : IStateCalculator<int>
    {
        public int CalculateState(Person person, int day)
        {
            return person.Assignments.AllRounds.ContainsKey(day - 1)
                ? 0
                : person.State.DayOffCount + 1;
        }

        public int InitializeState(Person person) => person.WorkSchedule.MinConsecutiveDayOffs - 1;
    }
}
