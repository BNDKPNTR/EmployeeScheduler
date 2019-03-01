using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class DateSpecificCover
    {
        public int Day { get; set; }

        [XmlElement("Cover")]
        public Cover[] Cover { get; set; }
    }
}
