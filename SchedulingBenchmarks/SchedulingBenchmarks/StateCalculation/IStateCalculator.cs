using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    interface IStateCalculator
    {
        object CalculateState(Person person, StateTriggers triggers, int day);
        object InitializeState(Person person);
    }

    interface IStateCalculator<out T> : IStateCalculator
    {
        new T CalculateState(Person person, StateTriggers triggers, int day);
        new T InitializeState(Person person);
    }
}
