using SchedulingBenchmarks.CostFunctions;
using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.StateCalculation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Schedulers
{
    internal class WeekendScheduler : SchedulerBase
    {
        private readonly CompositeStateCalculator _stateCalculator;

        public WeekendScheduler(SchedulerModel model, CostFunctionBase costFunction, WorkEligibilityChecker workEligibilityChecker, CompositeStateCalculator stateCalculator) 
            : base(model, costFunction, workEligibilityChecker)
        {
            _stateCalculator = stateCalculator ?? throw new ArgumentNullException(nameof(stateCalculator));
        }

        public override void Run()
        {
            Parallel.ForEach(_model.People, _model.ParallelOptions, person =>
            {
                person.Assignments.StartNewRound();
                _stateCalculator.InitializeState(person);
            });

            foreach (var day in _model.SchedulePeriod)
            {
                Parallel.ForEach(_model.People, _model.ParallelOptions, person => _stateCalculator.RefreshState(person, day));

                if (_model.Calendar.IsWeekend(day))
                {
                    var availablePeople = SelectPeopleForDay(day);
                    var demands = SelectDemandsForDay(day);

                    if (availablePeople.Count > 0 && demands.Length > 0)
                    {
                        SchedulePeople(day, availablePeople, demands);
                    }
                }
            }
        }
    }
}
