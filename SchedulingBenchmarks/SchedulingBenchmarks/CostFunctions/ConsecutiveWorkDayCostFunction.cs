using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class ConsecutiveShiftCostFunction : CostFunctionBase
    {
        private readonly Calendar _calendar;
        private readonly double _underMinConsecutiveShiftCount;
        private readonly double _betweenMinAndMaxShiftCount;
        private readonly double _workStartMultiplier;

        public ConsecutiveShiftCostFunction(Calendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
            _underMinConsecutiveShiftCount = DefaultCost / 100.0;
            _betweenMinAndMaxShiftCount = DefaultCost * 0.5;
            _workStartMultiplier = 2;
        }

        public override double CalculateCost(Person person, Demand demand, int day)
        {
            // If not working consecutively
            if (person.State.ConsecutiveWorkDayCount == 0) return CalculatePossibleWorkStartMultiplier(person);

            // In case of the min consecutive shifts we assume that before the schedule period there was an infinite number of shifts
            // TODO: same assumption applies to the end of the schedule period
            var schedulePeriodStartFilter = person.WorkSchedule.MinConsecutiveWorkDays;
            if (day > schedulePeriodStartFilter && person.State.ConsecutiveWorkDayCount < person.WorkSchedule.MinConsecutiveWorkDays) return _underMinConsecutiveShiftCount;

            return _betweenMinAndMaxShiftCount;
        }

        private double CalculatePossibleWorkStartMultiplier(Person person)
        {
            var ratio = person.State.PossibleFutureWorkDayCount / (double)person.WorkSchedule.MaxConsecutiveWorkDays;

            return DefaultCost + (1.0 - ratio) * _workStartMultiplier;
        }
    }
}
