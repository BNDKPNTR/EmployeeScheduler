﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class CoverMin
    {
        [XmlAttribute("weight")]
        public int Weight { get; set; }

        [XmlText]
        public int Value { get; set; }
    }
}
