using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Cli
{
    public class ExpectedTestResult
    {
        public int ExpectedPenalty { get; set; }
        public bool ExpectedFeasibility { get; set; }
        public bool ExpectedMaxNumberOfShiftsFeasibility { get; set; }
        public bool ExpectedMinTotalMinsFeasibility { get; set; }
        public bool ExpectedMaxTotalMinsFeasibility { get; set; }
        public bool ExpectedMinConsecutiveShiftsFeasibility { get; set; }
        public bool ExpectedMaxConsecutiveShiftsFeasibility { get; set; }
        public bool ExpectedMinConsecutiveDaysOffFeasibility { get; set; }
        public bool ExpectedMaxNumberOfWeekendsFeasibility { get; set; }
        public bool ExpectedDayOffsFeasibility { get; set; }
        public bool ExpectedMinRestTimeFeasibility { get; set; }
    }
}
