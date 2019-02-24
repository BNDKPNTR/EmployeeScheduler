using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class EmployeeAssign
    {
        public string Shift { get; set; }

        public int Day { get; set; }
    }
}
