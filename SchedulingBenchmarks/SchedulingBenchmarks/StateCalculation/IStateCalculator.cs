using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    interface IStateCalculator
    {
        string[] StatePropertyDependencies { get; }
        object CalculateState(Person person, State newState, StateTriggers triggers, int day);
        object InitializeState(Person person, State newState);
    }

    interface IStateCalculator<out T> : IStateCalculator
    {
        new T CalculateState(Person person, State newState, StateTriggers triggers, int day);
        new T InitializeState(Person person, State newState);
    }
}
