using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Models
{
    class Demand
    {
        public Range Period { get; set; }
        public Activity Activity { get; }
        public int RequiredPeopleCount { get; }

        public Demand(Range period, Activity activity, int requiredPeopleCount)
        {
            Period = period;
            Activity = activity ?? throw new ArgumentNullException(nameof(activity));
            RequiredPeopleCount = requiredPeopleCount;
        }
    }
}
