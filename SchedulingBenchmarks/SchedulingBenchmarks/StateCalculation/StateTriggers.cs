using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    class StateTriggers
    {
        public bool WorkedOnPreviousDay { get; }

        public StateTriggers(Person person, int day)
        {
            WorkedOnPreviousDay = person.Assignments.AllRounds.ContainsKey(day - 1);
        }
    }
}
