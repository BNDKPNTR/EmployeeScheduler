using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Dto
{
    public class Demand
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Activity Activity { get; set; }
        public int RequiredPersonCount { get; set; }
    }
}
