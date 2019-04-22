using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Demand
    {
        public int Day { get; set; }
        public Shift Shift { get; set; }
        public int RequiredPeopleCount { get; set; }

        public Demand(int day, Shift shift, int requiredPeopleCount)
        {
            Day = day;
            Shift = shift ?? throw new ArgumentNullException(nameof(shift));
            RequiredPeopleCount = requiredPeopleCount;
        }
    }
}
