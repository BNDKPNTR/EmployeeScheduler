using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Assignment
    {
        public Person Person { get; }
        public int TimeSlot { get; }
        public Activity Activity { get; }

        public Assignment(Person person, int timeSlot, Activity activity)
        {
            Person = person ?? throw new ArgumentNullException(nameof(person));
            TimeSlot = timeSlot;
            Activity = activity ?? throw new ArgumentNullException(nameof(activity));
        }

        public override string ToString() => $"{TimeSlot} - {Person.Name} - {Activity.Name}";
    }
}
