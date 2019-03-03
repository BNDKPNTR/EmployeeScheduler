using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;

namespace SchedulingBenchmarks.Cli
{
    class AlgorithmResult
    {
        public string Name { get; set; }
        public SchedulingBenchmarkModel Result { get; set; }
        public int Penalty { get; set; }
        public bool Feasible { get; set; }
        public List<string> FeasibilityMessages { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
