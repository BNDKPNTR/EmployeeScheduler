using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Demand
    {
        public int Day { get; set; }
        public string ShifId { get; set; }
        public int MinPeopleCount { get; }
        public int MaxPeopleCount { get; set; }

        public Demand(int day, string shiftId, int minPeopleCount, int maxPeopleCount)
        {
            Day = day;
            ShifId = shiftId ?? throw new ArgumentNullException(nameof(shiftId));
            MinPeopleCount = minPeopleCount;
            MaxPeopleCount = maxPeopleCount;
        }
    }
}
