using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Dto
{
    public class InputModel
    {
        public DateTime SchedulePeriodStart { get; set; }
        public DateTime SchedulePeriodEnd { get; set; }
        public int TimeSlotLengthInMinutes { get; set; }
        public List<Person> People { get; set; }
        public List<Demand> Demands { get; set; }
    }
}
