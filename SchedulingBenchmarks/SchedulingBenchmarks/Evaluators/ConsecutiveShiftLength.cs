using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Evaluators
{
    public class ConsecutiveShiftLength
    {
        public int DayStart { get; }
        public int Length { get; }

        public ConsecutiveShiftLength(int dayStart, int length)
        {
            DayStart = dayStart;
            Length = length;
        }
    }
}
