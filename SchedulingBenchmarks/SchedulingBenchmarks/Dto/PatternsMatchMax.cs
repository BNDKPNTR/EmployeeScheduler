using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class PatternsMatchMax
    {
        public byte Count { get; set; }

        public string Label { get; set; }
    }
}
