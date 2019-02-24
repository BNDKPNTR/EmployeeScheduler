using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class DateSpecificCover
    {
        public byte Day { get; set; }

        public DateSpecificCoverCover Cover { get; set; }
    }
}
