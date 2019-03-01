using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class Match
    {
        public MatchMax Max { get; set; }

        [XmlElement("Pattern")]
        public Pattern[] Pattern { get; set; }
    }
}
