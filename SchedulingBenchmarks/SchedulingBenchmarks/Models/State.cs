using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class State
    {
        public int TimeSlotsWorked { get; set; }
        public int TimeSlotsWorkedToday { get; set; }
        public int WorkedDaysInMonthCount { get; set; }
        public ImmutableDictionary<int, int> DailyWorkStartCounts { get; set; }
    }
}
