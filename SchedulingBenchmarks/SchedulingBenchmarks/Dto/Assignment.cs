using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class Assignment
    {
        [XmlElement("EmployeeID")]
        public string EmployeeId { get; set; }

        public EmployeeAssign Assign { get; set; }
    }
}
