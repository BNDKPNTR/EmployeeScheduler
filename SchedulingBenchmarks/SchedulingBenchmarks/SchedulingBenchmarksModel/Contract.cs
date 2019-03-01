using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.SchedulingBenchmarksModel
{
    public class Contract
    {
        public int MinRestTime { get; set; }
        public int MinTotalWorkTime { get; }
        public int MaxTotalWorkTime { get; }
        public int MinConsecutiveShifts { get; }
        public int MaxConsecutiveShifts { get; }
        public int MinConsecutiveDayOffs { get; }
    }
}
