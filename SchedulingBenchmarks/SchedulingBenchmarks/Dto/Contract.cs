using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Contract
    {
        public MaxSeq MaxSeq { get; set; }

        [XmlElement("MinSeq")]
        public MinSeq[] MinSeq { get; set; }

        [XmlArrayItem("TimeUnits", IsNullable = false)]
        public TimeUnits[] Workload { get; set; }

        public Patterns Patterns { get; set; }

        public ValidShifts ValidShifts { get; set; }

        public MinRestTime MinRestTime { get; set; }

        [XmlAttribute()]
        public string ID { get; set; }
    }
}
