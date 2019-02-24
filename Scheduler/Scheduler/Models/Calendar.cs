using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Models
{
    public class Calendar
    {
        /// <summary>
        /// A 0. indexű időszelet kezdetének pontos dátuma
        /// </summary>
        private readonly DateTime dateTimeOf0thTimeSlot;

        /// <summary>
        /// Egy referenciának vehető relatív naptár szerinti hétfő 0:00 időszelet indexe
        /// </summary>
        private readonly int referenceMondayDayStartIndex;

        /// <summary>
        /// Az időeltolás mértéke hétfő 0:00-hoz képest időszeletekben
        /// </summary>
        private readonly int dayShiftInTimeSlots;

        /// <summary>
        /// Időszelet hossz percben
        /// </summary>
        public int TimeSlotLengthInMinutes { get; }

        /// <summary>
        /// Időszelet hossz órában
        /// </summary>
        public int TimeSlotLengthInHour => TimeSlotLengthInMinutes / 60;

        /// <summary>
        /// Egy nap hossza időszeletekben
        /// </summary>
        public int TimeSlotsInADay { get; }

        /// <summary>
        /// Egy hét hossza időszeletekben
        /// </summary>
        public int TimeSlotsInAWeek { get; }

        /// <param name="dateTimeOf0thTimeSlot">A 0. indexű időszelet kezdetének pontos dátuma</param>
        /// <param name="timeSlotLengthInMinutes">Időszelet hossz percben</param>
        /// <param name="dayShiftInTimeSlots">Az időeltolás mértéke hétfő 0:00-hoz képest időszeletekben</param>
        public Calendar(DateTime dateTimeOf0thTimeSlot, int timeSlotLengthInMinutes, int dayShiftInTimeSlots = 0)
        {
            this.dateTimeOf0thTimeSlot = dateTimeOf0thTimeSlot;
            this.dayShiftInTimeSlots = dayShiftInTimeSlots;
            this.TimeSlotLengthInMinutes = timeSlotLengthInMinutes;
            this.TimeSlotsInADay = 24 * 60 / timeSlotLengthInMinutes;
            this.TimeSlotsInAWeek = TimeSlotsInADay * 7;

            this.referenceMondayDayStartIndex = CalculateReferenceMondayDayStartIndex();
        }

        /// <summary>
        /// Megadja az adott időszelet napjának első időszeletét
        /// </summary>
        public int DayStartOf(int timeSlotIndex)
        {
            var timeSlotsSinceRelativeDayStart = (timeSlotIndex - referenceMondayDayStartIndex) % TimeSlotsInADay;

            if (timeSlotsSinceRelativeDayStart < 0)
            {
                /* Ha negatív távolságra vagyunk a relatív napkezdettől, akkor igazából mögötte vagyunk, és a nekünk kellő 
                 * relatív napkezdet az eggyel korábbi nap. Az pedig pont olyan távolságra van, mint egy napnyi időszelet - amennyi van a következő napig. */
                timeSlotsSinceRelativeDayStart = timeSlotsSinceRelativeDayStart + TimeSlotsInADay;
            }

            return timeSlotIndex - timeSlotsSinceRelativeDayStart;
        }

        /// <summary>
        /// Megadja az adott időszelet napjának utolsó időszeletét (inkluzív)
        /// </summary>
        public int DayEndOf(int timeSlotIndex) => DayStartOf(timeSlotIndex) + TimeSlotsInADay - 1;

        /// <summary>
        /// Megadja az adott időszelet napjának időszelet intervallumát (inkluzív)
        /// </summary>
        public Range DayOf(int timeSlotIndex) => Range.Of(start: DayStartOf(timeSlotIndex), length: TimeSlotsInADay);

        /// <summary>
        /// Megadja az adott időszelet hetének első időszeletét
        /// </summary>
        public int WeekStartOf(int timeSlotIndex)
        {
            var timeSlotsSinceRelativeWeekStart = (timeSlotIndex - referenceMondayDayStartIndex) % TimeSlotsInAWeek;

            if (timeSlotsSinceRelativeWeekStart < 0)
            {
                /* Ha negatív távolságra vagyunk a relatív hétkezdettől, akkor igazából mögötte vagyunk, és a nekünk kellő 
                 * relatív hétkezdet az eggyel korábbi hét. Az pedig pont olyan távolságra van, mint egy hétnyi időszelet - amennyi van a következő hétig. */
                timeSlotsSinceRelativeWeekStart = timeSlotsSinceRelativeWeekStart + TimeSlotsInAWeek;
            }

            return timeSlotIndex - timeSlotsSinceRelativeWeekStart;
        }

        /// <summary>
        /// Megadja az adott időszelet hetének utolsó időszeletét (inkluzív)
        /// </summary>
        public int WeekEndOf(int timeSlotIndex) => WeekStartOf(timeSlotIndex) + TimeSlotsInAWeek - 1;

        /// <summary>
        /// Megadja az adott időszelet hetének időszelet intervallumát (inkluzív)
        /// </summary>
        public Range WeekOf(int timeSlotIndex) => Range.Of(start: WeekStartOf(timeSlotIndex), length: TimeSlotsInAWeek);

        /// <summary>
        /// Megadja az adott időszelet hónapjának első időszeletét
        /// </summary>
        public int MonthStartOf(int timeSlotIndex)
        {
            var timeSlotStartDate = TimeSlotIndexToDateTime(timeSlotIndex);
            var relativeTimeSlotStartDate = timeSlotStartDate.AddMinutes(-(dayShiftInTimeSlots * TimeSlotLengthInMinutes));
            var relativeMonthStartDate = new DateTime(relativeTimeSlotStartDate.Year, relativeTimeSlotStartDate.Month, 1) + TimeSpan.FromMinutes(dayShiftInTimeSlots * TimeSlotLengthInMinutes);

            return DateTimeToTimeSlotIndex(relativeMonthStartDate);
        }

        /// <summary>
        /// Megadja az adott időszelet hónapjának utolsó időszeletét (inkluzív)
        /// </summary>
        public int MonthEndOf(int timeSlotIndex)
        {
            var timeSlotStartDate = TimeSlotIndexToDateTime(timeSlotIndex);
            var relativeTimeSlotStartDate = timeSlotStartDate.AddMinutes(-(dayShiftInTimeSlots * TimeSlotLengthInMinutes));
            var relativeMonthStartDate = new DateTime(relativeTimeSlotStartDate.Year, relativeTimeSlotStartDate.Month, 1).AddMonths(1) - TimeSpan.FromMinutes(TimeSlotLengthInMinutes) + TimeSpan.FromMinutes(dayShiftInTimeSlots * TimeSlotLengthInMinutes);

            return DateTimeToTimeSlotIndex(relativeMonthStartDate);
        }

        /// <summary>
        /// Megadja az adott időszelet hónapjának időszelet intervallumát (inkluzív)
        /// </summary>
        public Range MonthOf(int timeSlotIndex) => Range.Of(MonthStartOf(timeSlotIndex), MonthEndOf(timeSlotIndex));

        /// <summary>
        /// Megadja az adott időszelet hónapjába eső vasárnapok intervallumait
        /// </summary>
        public IEnumerable<Range> SundaysOfMonth(int timeSlotIndex)
        {
            var month = MonthOf(timeSlotIndex);
            var weekStartOfMonth = WeekStartOf(month.Start);

            return Range.Of(weekStartOfMonth, month.End)
                .Split(TimeSlotsInAWeek)
                .Select(week => week.Mutate(start: week.Start + TimeSlotsInADay * 6, length: TimeSlotsInADay));
        }

        /// <summary>
        /// Az adott dátummal kezdődő időszelet indexét adja vissza
        /// </summary>
        public int DateTimeToTimeSlotIndex(DateTime dateTime) => MinutesToTimeSlots((dateTime - dateTimeOf0thTimeSlot).TotalMinutes);

        /// <summary>
        /// Az időszelethez tartozó kezdőidőpontot adja vissza abszolút naptár szerint
        /// </summary>
        public DateTime TimeSlotIndexToDateTime(int index) => dateTimeOf0thTimeSlot.AddMinutes(TimeSlotLengthInMinutes * index);

        public int MinutesToTimeSlots(double minutes) => (int)(minutes / TimeSlotLengthInMinutes);

        public int HoursToTimeSlots(double hours) => MinutesToTimeSlots(hours * 60);

        private int CalculateReferenceMondayDayStartIndex()
        {
            /* DayOfWeek numerikus értékei: Hétfő-Szombat: 1-6, Vasárnap: 0
             * Példa: 0. időszelet dátuma legyen kedd 6:00. Kivonok belőle 2 napot (kedd numerikus értéke), vasárnapot kapok. Hozzáadok egy napot,
             * és levágom az idő komponenst. */
            var referenceMondayDayStart = dateTimeOf0thTimeSlot.AddDays(-(int)dateTimeOf0thTimeSlot.DayOfWeek).AddDays(1).Date;
            return DateTimeToTimeSlotIndex(referenceMondayDayStart) + dayShiftInTimeSlots;
        }
    }
}
