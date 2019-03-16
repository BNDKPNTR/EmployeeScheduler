using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class WorkSchedule
    {
        public int MinRestTime { get; }
        public int MinTotalWorkTime { get; }
        public int MaxTotalWorkTime { get; }
        public int MinConsecutiveWorkDays { get; }
        public int MaxConsecutiveWorkDays { get; }
        public int MinConsecutiveDayOffs { get; }
        public int MaxWorkingWeekendCount { get; }
        public HashSet<Shift> ValidShifts { get; }
        public Dictionary<Shift, int> MaxShifts { get; }

        public WorkSchedule(int minRestTime, int minTotalWorkTime, int maxTotalWorkTime, int minConsecutiveWorkDays, int maxConsecutiveWorkDays, int minConsecutiveDayOffs, int maxWorkingWeekendCount, HashSet<Shift> validShifts, Dictionary<Shift, int> maxShifts)
        {
            MinRestTime = minRestTime;
            MinTotalWorkTime = minTotalWorkTime;
            MaxTotalWorkTime = maxTotalWorkTime;
            MinConsecutiveWorkDays = minConsecutiveWorkDays;
            MaxConsecutiveWorkDays = maxConsecutiveWorkDays;
            MinConsecutiveDayOffs = minConsecutiveDayOffs;
            MaxWorkingWeekendCount = maxWorkingWeekendCount;
            ValidShifts = validShifts ?? throw new ArgumentNullException(nameof(validShifts));
            MaxShifts = maxShifts ?? throw new ArgumentNullException(nameof(maxShifts));
        }
    }
}
