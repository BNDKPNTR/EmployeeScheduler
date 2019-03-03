using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Demand
    {
        public int Day { get; set; }
        public Shift Shift { get; set; }
        public int MinPeopleCount { get; }
        public int MaxPeopleCount { get; set; }

        public Demand(int day, Shift shift, int minPeopleCount, int maxPeopleCount)
        {
            Day = day;
            Shift = shift ?? throw new ArgumentNullException(nameof(shift));
            MinPeopleCount = minPeopleCount;
            MaxPeopleCount = maxPeopleCount;
        }
    }
}
