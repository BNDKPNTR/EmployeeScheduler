﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class WorkSchedule
    {
        public const int ShiftLengthInMinutes = 480;

        public int MinRestTime { get; }
        public int MinTotalWorkTime { get; }
        public int MaxTotalWorkTime { get; }
        public int MinConsecutiveShifts { get; }
        public int MaxConsecutiveShifts { get; }
        public int MinConsecutiveDayOffs { get; }
        public int MaxWorkingWeekendCount { get; }
        public HashSet<Shift> ValidShifts { get; }
        public Dictionary<Shift, int> MaxShifts { get; }

        public WorkSchedule(int minRestTime, int minTotalWorkTime, int maxTotalWorkTime, int minConsecutiveShifts, int maxConsecutiveShifts, int minConsecutiveDayOffs, int maxWorkingWeekendCount, HashSet<Shift> validShifts, Dictionary<Shift, int> maxShifts)
        {
            MinRestTime = minRestTime;
            MinTotalWorkTime = minTotalWorkTime;
            MaxTotalWorkTime = maxTotalWorkTime;
            MinConsecutiveShifts = minConsecutiveShifts;
            MaxConsecutiveShifts = maxConsecutiveShifts;
            MinConsecutiveDayOffs = minConsecutiveDayOffs;
            MaxWorkingWeekendCount = maxWorkingWeekendCount;
            ValidShifts = validShifts ?? throw new ArgumentNullException(nameof(validShifts));
            MaxShifts = maxShifts ?? throw new ArgumentNullException(nameof(maxShifts));
        }
    }
}
