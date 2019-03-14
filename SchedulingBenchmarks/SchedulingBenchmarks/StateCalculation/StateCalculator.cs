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
                new ConsecutiveShiftCountStateCalculator(),
                new DayOffCountStateCalculator(),
                new WorkedWeekendCountStateCalculator(schedulePeriod, calendar),
                new ShiftWorkedCountStateCalculator(),
                new NumberOfDaysCanWorkLaterStateCalculator(schedulePeriod),
            };
        }

        public void RefreshState(Person person, int day)
        {
            var results = new List<IStateCalculatorResult>(_stateCalculators.Length);

            foreach (var stateCalculator in _stateCalculators)
            {
                var result = stateCalculator.CalculateState(person, day);
                results.Add(result);
            }

            foreach (var result in results)
            {
                result.Apply(person);
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
                result.Apply(person);
            }
        }
    }
}
