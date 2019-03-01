using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.SchedulingBenchmarksModel
{
    public class Demand
    {
        public int Day { get; set; }
        public string ShiftId { get; set; }
        public int MinEmployeeCount { get; set; }
        public int MaxEmployeeCount { get; set; }
        public int UnderMinEmployeeCountPenalty { get; set; }
        public int OverMaxEmployeeCountPenalty { get; set; }
    }
}
