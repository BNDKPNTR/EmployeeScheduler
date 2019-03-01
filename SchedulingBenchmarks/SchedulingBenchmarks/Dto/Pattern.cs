using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class Pattern
    {
        public string StartDay { get; set; }

        [XmlElement("Shift")]
        public string[] Shift { get; set; }
    }
}
