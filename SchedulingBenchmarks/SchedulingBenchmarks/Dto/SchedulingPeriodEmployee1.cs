using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class SchedulingPeriodEmployee1
    {
        public string EmployeeID { get; set; }

        public EmployeeAssign Assign { get; set; }
    }
}
