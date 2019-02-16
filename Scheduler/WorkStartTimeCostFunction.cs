using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Scheduler
{
    class WorkStartTimeCostFunction
    {
        private const double DefaultCost = 1.0;
        private const double LessThanTwoThirdStartsAtTheSameTimeCost = 2.0;
        private const double TwoThird = 2.0 / 3.0;

        private readonly Calendar calendar;

        public WorkStartTimeCostFunction(Calendar calendar)
        {
            this.calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

        public double CalculateCost(Person person, Demand demand, int timeSlot)
        {
            if (person.State.TimeSlotsWorked > 0) return DefaultCost;

            var mostCommonWorkStartTime = GetMostCommonWorkStartTime(person, timeSlot);
            var workedDaysInMonthCount = person.State.TimeSlotsWorkedToday > 0 
                ? person.State.WorkedDaysInMonthCount 
                : person.State.WorkedDaysInMonthCount + 1;

            return mostCommonWorkStartTime / workedDaysInMonthCount < TwoThird
                ? LessThanTwoThirdStartsAtTheSameTimeCost
                : DefaultCost;
        }

        private int GetMostCommonWorkStartTime(Person person, int timeSlot)
        {
            var elapsedTimeSlotsSinceDayStart = timeSlot - calendar.DayStartOf(timeSlot);
            
            person.State.DailyWorkStartCounts.TryGetValue(elapsedTimeSlotsSinceDayStart, out var workStartCount);
            var dailyWorkStartCounts = person.State.DailyWorkStartCounts.SetItem(elapsedTimeSlotsSinceDayStart, workStartCount + 1);

            return dailyWorkStartCounts.Values.Max();
        }
    }
}
