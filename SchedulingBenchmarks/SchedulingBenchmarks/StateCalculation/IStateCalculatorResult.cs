using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.StateCalculation
{
    interface IStateCalculatorResult
    {
        void Apply(State state);
    }
}
