using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class ShiftOn
    {
        public string Shift { get; set; }

        public string EmployeeID { get; set; }

        public int Day { get; set; }

        [XmlAttribute("weight")]
        public int Weight { get; set; }
    }
}
