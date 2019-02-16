using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    class StateCalculator
    {
        private readonly Calendar _calendar;

        public StateCalculator(Calendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

        public void RefreshState(Person person, int timeSlot)
        {
            person.Availabilities.MoveNext(timeSlot);

            if (IsNewDay(timeSlot))
            {
                if (WorkedInPeriod(Range.Of(0, 0), person))
                {
                    person.State.WorkedDaysInMonth++;
                }
            }
        }

        private bool IsNewDay(int timeSlot) => _calendar.DayStartOf(timeSlot) == timeSlot;

        private bool WorkedInPeriod(Range period, Person person) => period.Any(timeSlot => person.Assignments.ContainsKey(timeSlot));
    }
}
