using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.SchedulingBenchmarksModel
{
    public class Employee
    {
        public string Id { get; set; }
        public Contract Contract { get; set; }
        public ShiftRequest[] ShiftOffRequests { get; set; }
        public ShiftRequest[] ShiftOnRequests { get; set; }
        public HashSet<int> DayOffs { get; set; }
    }
}
