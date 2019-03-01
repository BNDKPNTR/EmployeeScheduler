using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.SchedulingBenchmarksModel
{
    public class Contract
    {
        public int MinRestTime { get; set; }
        public int MinTotalWorkTime { get; set; }
        public int MaxTotalWorkTime { get; set; }
        public int MinConsecutiveShifts { get; set; }
        public int MaxConsecutiveShifts { get; set; }
        public int MinConsecutiveDayOffs { get; set; }
    }
}
