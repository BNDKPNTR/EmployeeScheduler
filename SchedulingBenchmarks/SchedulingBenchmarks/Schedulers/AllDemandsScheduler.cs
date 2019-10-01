using SchedulingBenchmarks.CostFunctions;
using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.StateCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Schedulers
{
    internal class AllDemandsScheduler : SchedulerBase
    {
        private readonly StateCalculator _stateCalculator;

        public AllDemandsScheduler(SchedulerModel model, CostFunctionBase costFunction, StateCalculator stateCalculator, WorkEligibilityChecker workEligibilityChecker)
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
