using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    interface IStateCalculator<out T> where T : IStateCalculatorResult
    {
        T CalculateState(Person person, StateTriggers triggers, int day);

        T InitializeState(Person person);
    }
}
