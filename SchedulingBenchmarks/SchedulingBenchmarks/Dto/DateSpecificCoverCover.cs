using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class DateSpecificCoverCover
    {
        public string Shift { get; set; }

        public DateSpecificCoverCoverMin Min { get; set; }

        public DateSpecificCoverCoverMax Max { get; set; }
    }
}
