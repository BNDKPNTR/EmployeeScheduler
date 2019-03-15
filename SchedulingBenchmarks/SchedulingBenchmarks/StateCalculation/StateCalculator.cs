using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    class StateCalculator
    {
        private readonly IStateCalculator<IStateCalculatorResult>[] _stateCalculators;

        public StateCalculator(Range schedulePeriod, Calendar calendar)
        {
            _stateCalculators = new IStateCalculator<IStateCalculatorResult>[]
            {
                new WorkedOnWeekedStateCalculator(calendar),
                new TotalWorkTimeStateCalculator(),
                new ConsecutiveWorkDayCountStateCalculator(),
                new ConsecutiveDayOffCountStateCalculator(),
                new WorkedWeekendCountStateCalculator(schedulePeriod, calendar),
                new ShiftWorkedCountStateCalculator(),
                new PossibleFutureWorkDayCountStateCalculator(),
            };
        }

        public void RefreshState(Person person, int day)
        {
            var results = new List<IStateCalculatorResult>(_stateCalculators.Length);
            var triggers = new StateTriggers(person, day);

            foreach (var stateCalculator in _stateCalculators)
            {
                var result = stateCalculator.CalculateState(person, triggers, day);
                results.Add(result);
            }

            foreach (var result in results)
            {
                result.Apply(person.State);
            }
        }

        public void InitializeState(Person person)
        {
            var results = new List<IStateCalculatorResult>(_stateCalculators.Length);

            foreach (var stateCalculator in _stateCalculators)
            {
                var result = stateCalculator.InitializeState(person);
                results.Add(result);
            }

            foreach (var result in results)
            {
                result.Apply(person.State);
            }
        }
    }
}
