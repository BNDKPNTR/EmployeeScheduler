using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class DateSpecificCoverCoverMin
    {
        [XmlAttribute()]
        public byte Weight { get; set; }

        [XmlText()]
        public byte Value { get; set; }
    }
}
