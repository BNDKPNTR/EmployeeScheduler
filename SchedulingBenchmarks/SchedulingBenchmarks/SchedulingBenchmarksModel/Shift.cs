using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.SchedulingBenchmarksModel
{
    public class Shift
    {
        public string Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
