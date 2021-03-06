﻿using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class WorkedWeekendCountStateCalculator : StateCalculatorBase<int>
    {
        private readonly Range _schedulePeriod;
        private readonly Calendar _calendar;

        public WorkedWeekendCountStateCalculator(Range schedulePeriod, Calendar calendar)
        {
            _schedulePeriod = schedulePeriod;
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

        public override int CalculateState(Person person, State newState, StateTriggers triggers, int day)
        {
            if (!_calendar.IsMonday(day)) return person.State.WorkedWeekendCount;

            return WorkedOnWeekendInThisRound(person, day)
                ? person.State.WorkedWeekendCount + 1
                : person.State.WorkedWeekendCount;
        }

        private bool WorkedOnWeekendInThisRound(Person person, int day)
        {
            return !person.Assignments.PreviousAssignments.ContainsKey(day - 2)
                && !person.Assignments.PreviousAssignments.ContainsKey(day - 1)
                && (person.Assignments.LatestRound.ContainsKey(day - 2)
                || person.Assignments.LatestRound.ContainsKey(day - 1));
        }

        public override int InitializeState(Person person, State newState)
        {
            var workedOnWeekendCount = 0;

            foreach (var (assignmentOnSaturday, assignmentOnSunday) in GetWeekendAssignments(person.Assignments))
            {
                if (assignmentOnSaturday != null || assignmentOnSunday != null)
                {
                    workedOnWeekendCount++;
                }
            }

            return workedOnWeekendCount;
        }

        private IEnumerable<(Assignment assignmentOnSaturday, Assignment assignmentOnSunday)> GetWeekendAssignments(AssignmentsCollection assignments)
        {
            for (int i = _schedulePeriod.Start + 5; i < _schedulePeriod.Length; i += 7)
            {
                assignments.AllRounds.TryGetValue(i, out var assignmentOnSaturday);
                assignments.AllRounds.TryGetValue(i + 1, out var assignmentOnSunday);

                yield return (assignmentOnSaturday, assignmentOnSunday);
            }
        }
    }
}
