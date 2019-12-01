using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    internal abstract class StateCalculatorBase<T> : IStateCalculator<T>
    {
        public abstract T CalculateState(Person person, StateTriggers triggers, int day);

        public abstract T InitializeState(Person person);

        object IStateCalculator.CalculateState(Person person, StateTriggers triggers, int day) => CalculateState(person, triggers, day);

        object IStateCalculator.InitializeState(Person person) => InitializeState(person);
    }
}
