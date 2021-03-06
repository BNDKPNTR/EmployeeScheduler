﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class ValidShifts
    {
        [XmlAttribute("shift")]
        public string Shift { get; set; }
    }
}
