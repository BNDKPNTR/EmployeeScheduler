using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.SchedulingBenchmarksModel
{
    public class Shift
    {
        public const string NoneShiftId = "-";
        public const string AnyShiftId = "$";

        public string Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
