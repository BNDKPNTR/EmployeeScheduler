using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Assignment
    {
        public Person Person { get; }
        public int TimeSlot { get; }
        public string ShiftId { get; }

        public Assignment(Person person, int timeSlot, string shiftId)
        {
            Person = person ?? throw new ArgumentNullException(nameof(person));
            TimeSlot = timeSlot;
            ShiftId = shiftId ?? throw new ArgumentNullException(nameof(shiftId));
        }

        public override string ToString() => $"{TimeSlot} - {Person.Id} - {ShiftId}";
    }
}
