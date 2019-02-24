using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Dto
{
    public class Assignment
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Person Person { get; set; }
        public Activity Activity { get; set; }
    }
}
