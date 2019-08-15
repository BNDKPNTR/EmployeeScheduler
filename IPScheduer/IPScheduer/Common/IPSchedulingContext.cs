﻿using System.Collections.Generic;

namespace IPScheduler.Common
{
    internal class IPSchedulingContext
    {
        public List<Person> Persons { get; } = new List<Person>();

        public List<Shift> Shifts { get; } = new List<Shift>();

        public int MyProperty { get; set; }

    }
}
