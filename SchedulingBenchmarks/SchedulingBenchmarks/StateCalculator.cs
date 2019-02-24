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

            if (WorkStartedInPreviousTimeSlot(person, timeSlot))
            {
                var previousTimeSlotIndex = timeSlot - 1;
                var elapsedTimeSlotsSinceDayStart = previousTimeSlotIndex - _calendar.DayStartOf(previousTimeSlotIndex);

                person.State.DailyWorkStartCounts.TryGetValue(elapsedTimeSlotsSinceDayStart, out var workStartCount);
                person.State.DailyWorkStartCounts = person.State.DailyWorkStartCounts.SetItem(elapsedTimeSlotsSinceDayStart, workStartCount + 1);

                person.State.WorkedDaysInMonthCount++;
            }

            person.State.TimeSlotsWorked = WorkedInPreviousTimeSlot(person, timeSlot)
                ? person.State.TimeSlotsWorked + 1
                : 0;

            person.State.TimeSlotsWorkedToday = IsNewDay(timeSlot)
                ? 0
                : (WorkedInPreviousTimeSlot(person, timeSlot) ? person.State.TimeSlotsWorkedToday + 1 : person.State.TimeSlotsWorkedToday);
        }

        private bool IsNewDay(int timeSlot) => _calendar.DayStartOf(timeSlot) == timeSlot;

        private bool WorkedInPreviousTimeSlot(Person person, int timeSlot) => person.Assignments.ContainsKey(timeSlot - 1);

        private bool WorkStartedInPreviousTimeSlot(Person person, int timeSlot) => person.State.TimeSlotsWorked == 0 && WorkedInPreviousTimeSlot(person, timeSlot);
    }
}
