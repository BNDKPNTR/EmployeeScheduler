using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class State
    {
        public int TotalWorkTime { get; set; }
        public int ConsecutiveWorkDayCount { get; set; }
        public int ConsecutiveDayOffCount { get; set; }
        public bool WorkedOnWeeked { get; set; }
        public int WorkedWeekendCount { get; set; }
        public Dictionary<Shift, int> ShiftWorkedCount { get; set; }
        public int PossibleFutureWorkDayCount { get; set; }
    }
}
