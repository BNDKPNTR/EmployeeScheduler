using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class TimeUnits
    {
        public TimeUnitsMin Min { get; set; }

        public TimeUnitsMax Max { get; set; }
    }
}
