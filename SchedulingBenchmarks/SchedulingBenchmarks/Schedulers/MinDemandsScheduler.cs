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
    internal class MinDemandsScheduler : SchedulerBase
    {
        private readonly StateCalculator _stateCalculator;

        public MinDemandsScheduler(SchedulerModel model, CostFunctionBase costFunction, WorkEligibilityChecker workEligibilityChecker, StateCalculator stateCalculator) 
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

                var availablePeople = SelectPeopleForMinDemands(day);
                var demands = SelectDemandsForDay(day);

                if (availablePeople.Count > 0 && demands.Length > 0 && availablePeople.Count <= demands.Length)
                {
                    SchedulePeople(day, availablePeople, demands);
                }
            }
        }

        private List<Person> SelectPeopleForMinDemands(int day)
            => _model.People
            .Where(p => p.Availabilities[day])
            .ToList();
    }
}
