using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [XmlRoot]
    public class SchedulingPeriod
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Employee[] Employees { get; set; }

        [XmlArray("ShiftTypes")]
        public Shift[] Shifts { get; set; }

        [XmlArrayItem("Contract", IsNullable = false)]
        public Contract[] Contracts { get; set; }

        [XmlArrayItem("Employee")]
        public Assignment[] FixedAssignments { get; set; }

        public ShiftOff[] ShiftOffRequests { get; set; }

        public ShiftOn[] ShiftOnRequests { get; set; }

        public DateSpecificCover[] CoverRequirements { get; set; }
    }
}
