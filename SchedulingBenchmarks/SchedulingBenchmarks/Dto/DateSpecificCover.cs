using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class DateSpecificCover
    {
        public int Day { get; set; }

        public DateSpecificCoverCover Cover { get; set; }
    }
}
