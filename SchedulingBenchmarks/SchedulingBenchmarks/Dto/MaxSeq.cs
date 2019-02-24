using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class MaxSeq
    {
        [XmlAttribute()]
        public string Label { get; set; }

        [XmlAttribute()]
        public byte Value { get; set; }

        [XmlAttribute()]
        public string Shift { get; set; }
    }
}
