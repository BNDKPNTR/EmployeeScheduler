using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class MaxTot
    {
        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlAttribute("shift")]
        public string Shift { get; set; }
    }
}
