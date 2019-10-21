using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.StateCalculation;

namespace SchedulingBenchmarks.CostFunctions
{
    class TotalWorkTimeCostFunction : CostFunctionBase
    {
        private readonly SchedulerModel _model;
        private readonly WorkEligibilityChecker _workEligibilityChecker;
        private readonly StateCalculator _stateCalculator;
        private readonly double _underMinWorkTimeCost;
        private readonly Shift _longestShift;

        public TotalWorkTimeCostFunction(SchedulerModel model, WorkEligibilityChecker workEligibilityChecker, StateCalculator stateCalculator)
        {
            _underMinWorkTimeCost = DefaultCost * 0.5;
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _workEligibilityChecker = workEligibilityChecker ?? throw new ArgumentNullException(nameof(workEligibilityChecker));
            _stateCalculator = stateCalculator ?? throw new ArgumentNullException(nameof(stateCalculator));
            _longestShift = _model.Demands.Values
                .SelectMany(dArr => dArr)
                .Select(d => d.Shift)
                .OrderByDescending(s => s.Duration)
                .First();
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            if (person.State.TotalWorkTime + demand.Shift.Duration > person.WorkSchedule.MaxTotalWorkTime) return MaxCost;
            if (!CanWorkMinTotalWorkTime(person, demand, day)) return MaxCost;
            if (person.State.TotalWorkTime < person.WorkSchedule.MinTotalWorkTime) return _underMinWorkTimeCost;

            return DefaultCost;
        }

        private bool CanWorkMinTotalWorkTime(Person person, Demand demand, int today)
        {
            var totalWorkTime = person.State.TotalWorkTime + demand.Shift.Duration;
            var remaingingDays = _model.SchedulePeriod.ExclusiveEnd - today;
            var maxPossibleTotalWorkTime = totalWorkTime + remaingingDays * _longestShift.Duration;

            if (maxPossibleTotalWorkTime < person.WorkSchedule.MinTotalWorkTime) return true;
            if (totalWorkTime >= person.WorkSchedule.MinTotalWorkTime) return true;
            if (today == _model.SchedulePeriod.End) return totalWorkTime >= person.WorkSchedule.MinTotalWorkTime;

            var originalState = person.State.Clone();
            var originalAssignments = person.Assignments.Clone();

            person.Assignments.Add(today, new Assignment(person, today, demand.Shift));
            var canWorkMinTotalWorkTime = false;

            foreach (var day in Range.Of(today + 1, _model.SchedulePeriod.End))
            {
                _stateCalculator.RefreshState(person, day);

                if (person.State.TotalWorkTime >= person.WorkSchedule.MinTotalWorkTime)
                {
                    canWorkMinTotalWorkTime = true;
                    break;
                }

                if (_workEligibilityChecker.CanWorkOnDay(person, day))
                {
                    person.Assignments.Add(day, new Assignment(person, day, _longestShift));
                }
            }

            _stateCalculator.RefreshState(person, _model.SchedulePeriod.End);
            canWorkMinTotalWorkTime = person.State.TotalWorkTime >= person.WorkSchedule.MinTotalWorkTime;

            person.State = originalState;
            person.Assignments = originalAssignments;

            return canWorkMinTotalWorkTime;
        }
    }
}
