using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class SchedulerModel
    {
        public Range SchedulePeriod { get; set; }
        public Calendar Calendar { get; set; }
        public List<Person> People { get; set; }
        public Demand[] Demands { get; set; }
    }
}
