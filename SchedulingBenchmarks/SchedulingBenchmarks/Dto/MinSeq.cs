using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class MinSeq
    {
        [XmlAttribute("label")]
        public string Label { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlAttribute("shift")]
        public string Shift { get; set; }
    }
}
