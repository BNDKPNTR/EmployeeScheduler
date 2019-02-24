using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class MinRestTime
    {
        [XmlAttribute()]
        public string Label { get; set; }

        [XmlText()]
        public ushort Value { get; set; }
    }
}
