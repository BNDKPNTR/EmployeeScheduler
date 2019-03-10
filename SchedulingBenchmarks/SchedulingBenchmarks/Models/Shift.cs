using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Shift
    {
        public int Index { get; }
        public string Id { get; }
        public int StartTime { get; }
        public int Duration { get; }

        public Shift(int index, string id, int startTime, int duration)
        {
            Index = index;
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Duration = duration;
            StartTime = startTime;
        }
    }
}
