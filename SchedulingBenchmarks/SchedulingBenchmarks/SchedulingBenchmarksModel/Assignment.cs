using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.SchedulingBenchmarksModel
{
    public class Assignment
    {
        public int Day { get; set; }
        public string ShiftId { get; set; }
        public string PersonId { get; set; }
    }
}
