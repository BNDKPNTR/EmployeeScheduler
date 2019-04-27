﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class TotalWorkTimeStateCalculator : IStateCalculator<int>
    {
        public int CalculateState(Person person, StateTriggers triggers, int day)
        {
            if (person.Assignments.LatestRound.TryGetValue(day - 1, out var assignment))
            {
                return person.State.TotalWorkTime + assignment.Shift.Duration;
            }

            return person.State.TotalWorkTime;
        }

        public int InitializeState(Person person) => person.Assignments.AllRounds.Values.Sum(a => a.Shift.Duration);
    }
}
