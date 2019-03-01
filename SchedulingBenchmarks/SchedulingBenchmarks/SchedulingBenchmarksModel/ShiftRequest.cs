using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.SchedulingBenchmarksModel
{
    public class ShiftRequest
    {
        public string ShiftId { get; set; }
        public int Day { get; set; }
        public int Penalty { get; set; }
    }
}
