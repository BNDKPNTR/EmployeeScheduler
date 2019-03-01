using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class Cover
    {
        public string Shift { get; set; }

        public CoverMin Min { get; set; }

        public CoverMax Max { get; set; }
    }
}
