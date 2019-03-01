using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.SchedulingBenchmarksModel
{
    public class SchedulingBenchmarkModel
    {
        public int Duration { get; set; }

        public Shift[] Shifts { get; set; }
        public Employee[] Employees { get; set; }
        public Dictionary<int, Demand[]> Demands { get; set; }
    }
}
