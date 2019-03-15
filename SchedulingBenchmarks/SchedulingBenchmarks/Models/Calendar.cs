using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Calendar
    {
        public bool IsSaturday(int day) => day % 7 == 5;

        public bool IsSunday(int day) => day % 7 == 6;

        public bool IsMonday(int day) => day % 7 == 0;

        public bool IsWeekend(int day) => IsSaturday(day) || IsSunday(day);
    }
}
