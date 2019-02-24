using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Dto
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Availability> Availabilities { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}
