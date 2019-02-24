using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class SchedulingPeriod
    {
        [XmlElement(DataType = "date")]
        public DateTime StartDate { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime EndDate { get; set; }

        [XmlArrayItem("Shift", IsNullable = false)]
        public Shift[] ShiftTypes { get; set; }

        [XmlArrayItem("Contract", IsNullable = false)]
        public Contract[] Contracts { get; set; }

        [XmlArrayItem("Employee", IsNullable = false)]
        public Employee[] Employees { get; set; }

        [XmlArrayItem("Employee", IsNullable = false)]
        public SchedulingPeriodEmployee1[] FixedAssignments { get; set; }

        [XmlArrayItem("ShiftOff", IsNullable = false)]
        public ShiftOff[] ShiftOffRequests { get; set; }

        [XmlArrayItem("ShiftOn", IsNullable = false)]
        public ShiftOn[] ShiftOnRequests { get; set; }

        [XmlArrayItem("DateSpecificCover", IsNullable = false)]
        public DateSpecificCover[] CoverRequirements { get; set; }
    }
}
