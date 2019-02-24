using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class PatternsMatch
    {
        public PatternsMatchMax Max { get; set; }

        [XmlElement("Pattern")]
        public PatternsMatchPattern[] Pattern { get; set; }
    }
}
