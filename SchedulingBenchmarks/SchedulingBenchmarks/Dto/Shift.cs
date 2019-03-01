using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class Shift
    {
        public const string NoneShiftId = "-";
        public const string AnyShiftId = "$";

        [XmlAttribute]
        public string ID { get; set; }
        
        [XmlIgnore]
        public string StartTime { get; set; }

        public int Duration { get; set; }
    }
}
