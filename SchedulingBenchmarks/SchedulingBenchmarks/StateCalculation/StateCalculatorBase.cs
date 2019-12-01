using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    internal abstract class StateCalculatorBase<T> : IStateCalculator<T>
    {
        public virtual string[] StatePropertyDependencies => Array.Empty<string>();

        public abstract T CalculateState(Person person, State newState, StateTriggers triggers, int day);

        public abstract T InitializeState(Person person, State newState);

        object IStateCalculator.CalculateState(Person person, State newState, StateTriggers triggers, int day) => CalculateState(person, newState, triggers, day);

        object IStateCalculator.InitializeState(Person person, State newState) => InitializeState(person, newState);
    }
}
