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
    internal class UntilMinTotalWorkTimeScheduler : SchedulerBase
    {
        private readonly StateCalculator _stateCalculator;

        public UntilMinTotalWorkTimeScheduler(SchedulerModel model, StateCalculator stateCalculator, WorkEligibilityChecker workEligibilityChecker, CostFunctionBase costFunction) : base(model, costFunction, workEligibilityChecker)
        {
            _stateCalculator = stateCalculator ?? throw new ArgumentNullException(nameof(stateCalculator));
        }

        public override void Run()
        {
            Parallel.ForEach(_model.People.Where(p => p.State.TotalWorkTime < p.WorkSchedule.MinTotalWorkTime), _model.ParallelOptions, person =>
            {
                person.Assignments.StartNewRound();
                _stateCalculator.InitializeState(person);

                foreach (var day in _model.SchedulePeriod)
                {
                    _stateCalculator.RefreshState(person, day);

                    if (!_workEligibilityChecker.CanWorkOnDay(person, day)) continue;
                    if (person.State.TotalWorkTime >= person.WorkSchedule.MinTotalWorkTime)
                    {
                        if (person.State.ConsecutiveWorkDayCount >= person.WorkSchedule.MinConsecutiveWorkDays
                        || person.State.TotalWorkTime >= person.WorkSchedule.MaxTotalWorkTime)
                        {
                            break;
                        }
                    }
                    if (person.Assignments.AllRounds.ContainsKey(day)) continue;

                    foreach (var demand in _model.Demands[day])
                    {
                        if (_costFunction.CalculateCost(person, demand, day) < _costFunction.MaxCost)
                        {
                            var assignment = new Assignment(person, day, demand.Shift);

                            person.Assignments.Add(day, assignment);
                            break;
                        }
                    }
                }
            });
        }
    }
}
