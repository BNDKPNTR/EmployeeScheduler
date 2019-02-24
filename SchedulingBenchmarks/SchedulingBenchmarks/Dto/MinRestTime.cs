using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class MinRestTime
    {
        [XmlAttribute]
        public string Label { get; set; }

        [XmlText]
        public int Value { get; set; }
    }
}
