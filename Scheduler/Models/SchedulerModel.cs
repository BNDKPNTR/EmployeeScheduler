﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Models
{
    class SchedulerModel
    {
        public Range SchedulePeriod { get; set; }
        public Calendar Calendar { get; set; }
        public List<Person> People { get; set; }
        public TimeSlotDependentCollection<List<Demand>> AllDemands { get; set; }
    }
}
