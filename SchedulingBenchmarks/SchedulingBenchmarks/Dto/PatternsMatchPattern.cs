using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class PatternsMatchPattern
    {
        public string StartDay { get; set; }

        [XmlElement("Shift")]
        public string[] Shift { get; set; }
    }
}
