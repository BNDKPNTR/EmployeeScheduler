using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class State
    {
        public int TotalWorkTime { get; set; }
        public int ConsecutiveShiftCount { get; set; }
        public int DayOffCount { get; set; }
        public bool WorkedOnWeeked { get; set; }
        public int WorkedWeekendCount { get; set; }
        public Dictionary<string, int> ShiftWorkedCount { get; set; }
    }
}
