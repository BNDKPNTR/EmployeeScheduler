using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Employee
    {
        [XmlElement("ContractID")]
        public string[] ContractID { get; set; }

        [XmlAttribute()]
        public string ID { get; set; }
    }
}
