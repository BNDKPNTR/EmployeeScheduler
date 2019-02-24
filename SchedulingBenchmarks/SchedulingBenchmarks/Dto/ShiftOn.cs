using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class ShiftOn
    {
        public string Shift { get; set; }

        public string EmployeeID { get; set; }

        public byte Day { get; set; }

        [XmlAttribute()]
        public byte Weight { get; set; }
    }
}
