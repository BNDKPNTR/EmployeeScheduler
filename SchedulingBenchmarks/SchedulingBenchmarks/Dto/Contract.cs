using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class Contract
    {
        public const string UniversalContractId = "All";

        [XmlAttribute]
        public string ID { get; set; }

        [XmlElement("MaxTot")]
        public MaxTot[] MaxTot { get; set; }

        public MaxSeq MaxSeq { get; set; }

        [XmlElement("MinSeq")]
        public MinSeq[] MinSeq { get; set; }

        [XmlArrayItem("TimeUnits")]
        public TimeUnits[] Workload { get; set; }

        public Patterns Patterns { get; set; }

        public ValidShifts ValidShifts { get; set; }

        public MinRestTime MinRestTime { get; set; }
    }
}
