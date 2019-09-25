using System;

namespace IPScheduler.Common
{
    public static class SchedulingGlobalConstants
    {
        public static DateTime StartDate { get; set; } = new DateTime(2000, 01, 01);
        public const int NullAmount = -2050;

        public const string AllShiftId = "$";
        public const string FreeDayShiftId = "-";
    }
}