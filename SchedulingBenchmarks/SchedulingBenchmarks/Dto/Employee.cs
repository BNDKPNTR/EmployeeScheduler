using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class Employee
    {
        [XmlElement("ContractID")]
        public string[] ContractIds { get; set; }

        [XmlAttribute("ID")]
        public string Id { get; set; }
    }
}
