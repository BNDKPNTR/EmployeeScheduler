﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class MaxSeq
    {
        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlAttribute("shift")]
        public string Shift { get; set; }
    }
}
