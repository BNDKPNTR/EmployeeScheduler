using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Shift
    {
        public string Color { get; set; }

        public string StartTime { get; set; }

        public int Duration { get; set; }

        [XmlAttribute]
        public string ID { get; set; }
    }
}
