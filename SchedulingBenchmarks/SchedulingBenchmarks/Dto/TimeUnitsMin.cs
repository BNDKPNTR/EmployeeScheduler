using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class TimeUnitsMin
    {
        public ushort Count { get; set; }

        public string Label { get; set; }
    }
}
